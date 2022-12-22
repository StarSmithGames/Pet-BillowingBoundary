using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DateTimeExtensions
{
	public static long TotalSeconds(this DateTime dateTime)
	{
		return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
	}
}