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

	public static T GetChildComponentByName<T>(this Transform transform, string name) where T : Component
	{
		foreach (T component in transform.GetComponentsInChildren<T>(true))
		{
			if (component.gameObject.name == name)
			{
				return component;
			}
		}
		return null;
	}
}