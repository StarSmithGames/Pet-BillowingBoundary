using System;
using System.Collections.Generic;

public static class CollectionExtensions
{
	public static T RandomItem<T>(this IList<T> list, int from = 0, int to = -1)
	{
		if (list.Count == 0) return default;
		return list[UnityEngine.Random.Range(from, to == -1 ? list.Count : to)];
	}

	/// <summary>
	/// Resize for listB by listA
	/// </summary>
	public static void Resize<FIRST, SECOND>(IList<FIRST> listA, IList<SECOND> listB, Func<SECOND> onCreate, Func<SECOND> onRemove = null)
	{
		Resize(listA.Count, listB, onCreate, onRemove);
	}

	/// <summary>
	/// Resize list by size
	/// </summary>
	public static void Resize<T>(int size, IList<T> list, Func<T> onCreate, Func<T> onRemove = null)
	{
		int diff = size - list.Count;

		if (diff != 0)
		{
			if (diff > 0)//add
			{
				for (int i = 0; i < diff; i++)
				{
					Add();
				}
			}
			else//rm
			{
				for (int i = 0; i < -diff; i++)
				{
					Remove();
				}
			}
		}

		void Add()
		{
			if (onCreate != null)
			{
				T item = onCreate.Invoke();
				list.Add(item);
			}
		}

		void Remove()
		{
			if (onRemove != null)
			{
				T item = onRemove.Invoke();
				list.Remove(item);
			}
		}
	}
}