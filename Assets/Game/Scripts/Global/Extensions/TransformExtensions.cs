using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
	public static Transform DestroyChildren(this Transform transform, bool isImmediate = false)
	{
		foreach (Transform child in transform)
		{
			if (isImmediate)
			{
				GameObject.DestroyImmediate(child.gameObject);
			}
			else
			{
				GameObject.Destroy(child.gameObject);
			}
		}
		return transform;
	}
}