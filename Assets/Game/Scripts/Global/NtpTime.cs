using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public static class GetTime
{
	public static double LocalTime()
	{
		return DateTime.UtcNow.Subtract(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
	}

	public static DateTime Now
	{
		get { return CurrentTime(); }
	}

	private static DateTime CurrentTime()
	{
		const string NTP_SERVER = "pool.ntp.org";
		var ntpData = new byte[48];
		ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

		try
		{
			var addresses = Dns.GetHostEntry(NTP_SERVER).AddressList;
			var ipEndPoint = new IPEndPoint(addresses[0], 123);
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			socket.Connect(ipEndPoint);
			socket.ReceiveTimeout = 3000;
			socket.Send(ntpData);
			socket.Receive(ntpData);
			socket.Close();
		}
		catch
		{
			Debug.LogError("Connection off!");
			return DateTime.Now;
		}
		//Offset to get to the "Transmit Timestamp" field (time at which the reply
		//departed the server for the client, in 64-bit timestamp format."
		const byte SERVER_REPLY_TIME = 40;

		//Get the seconds part
		ulong intPart = BitConverter.ToUInt32(ntpData, SERVER_REPLY_TIME);

		//Get the seconds fraction
		ulong fractPart = BitConverter.ToUInt32(ntpData, SERVER_REPLY_TIME + 4);

		//Convert From big-endian to little-endian
		intPart = SwapEndianness(intPart);
		fractPart = SwapEndianness(fractPart);

		ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
		DateTime networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds(milliseconds);

		return networkDateTime.ToLocalTime();
	}


	private static uint SwapEndianness(ulong x)
	{
		return (uint)(((x & 0x000000ff) << 24) +
						((x & 0x0000ff00) << 8) +
						((x & 0x00ff0000) >> 8) +
						((x & 0xff000000) >> 24));
	}
}