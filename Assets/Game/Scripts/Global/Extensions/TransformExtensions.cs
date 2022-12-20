using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public static class TransformExtensions
{
	public static Transform DestroyChildren(this Transform transform, bool isImmediate = false)
	{
		var tempList = transform.Cast<Transform>().ToList();
		foreach (Transform child in tempList)
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