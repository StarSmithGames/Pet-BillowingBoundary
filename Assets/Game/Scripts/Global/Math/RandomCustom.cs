
using System;

public static class RandomCustom
{
    public static double GetRandomNumber(double min, double max)
	{
		Random random = new Random();
		return random.NextDouble() * (max - min) + min;
	}

	public static long GetRandomNumber(long min, long max)
	{
		Random random = new Random();
		byte[] buf = new byte[8];
		random.NextBytes(buf);
		long longRand = BitConverter.ToInt64(buf, 0);

		return (Math.Abs(longRand % (max - min)) + min);
	}
}