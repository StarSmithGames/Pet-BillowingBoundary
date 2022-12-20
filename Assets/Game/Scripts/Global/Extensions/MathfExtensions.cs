using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathfExtensions
{
	public static float RandomBtw(this Vector2 vector2)
	{
		return Random.Range(vector2.x, vector2.y);
	}
}