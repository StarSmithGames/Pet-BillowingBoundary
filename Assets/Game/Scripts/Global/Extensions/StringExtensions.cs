using System;
using System.Linq;

public static class StringExtensions
{
	public static bool IsEmpty(this string s)
	{
		return string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s);
	}

	public static string FirstCharToUpper(this string input) =>
	input switch
	{
		null => throw new ArgumentNullException(nameof(input)),
		"" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
		_ => input[0].ToString().ToUpper() + input.Substring(1)
	};
}