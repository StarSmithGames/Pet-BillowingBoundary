using Sirenix.OdinInspector;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyTest : MonoBehaviour
{
	public float lastOpened;
	public float nextDay;

	[ShowInInspector]
	public string LastOpened => ConvertFromUnixTimestamp(lastOpened).ToString();
	[ShowInInspector]
	public string NexDay => ConvertFromUnixTimestamp(nextDay).ToString();

	[ShowInInspector]
	public string Diff => (nextDay - lastOpened).ToString();
	[ShowInInspector]
	public string DiffSpan => TimeSpan.FromSeconds(nextDay - lastOpened).ToString();

	[ShowInInspector]
	public string DiffDay => (nextDay - DateTime.UtcNow.TotalSeconds()).ToString();//сколько осталось
	[ShowInInspector]
	public string DiffDaySpan => TimeSpan.FromSeconds(nextDay - DateTime.UtcNow.TotalSeconds()).ToString();



	[Button(DirtyOnClick = true)]
	private void Open()
	{
		var now = DateTime.UtcNow;
		lastOpened = now.TotalSeconds();
		now = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
		nextDay = now.AddDays(1).TotalSeconds();//next day
	}

	public static DateTime ConvertFromUnixTimestamp(double timestamp)
	{
		DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return origin.AddSeconds(timestamp);
	}

}