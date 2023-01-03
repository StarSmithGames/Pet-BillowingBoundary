using System;
using System.Collections.Generic;
using System.Linq;

public static class CollectionExtensions
{
	public static T RandomItem<T>(this IList<T> list, int from = 0, int to = -1)
	{
		if (list.Count == 0) return default;
		return list[UnityEngine.Random.Range(from, to == -1 ? list.Count : to)];
	}

	public static T RandomItem<T>(this IList<T> list, int from = 0, int to = -1, params T[] except)
	{
		if (list.Count == 0) return default;

		var check = list.Except(except).ToList();
		return check[UnityEngine.Random.Range(from, to == -1 ? check.Count : to)];
	}

	public static List<T> Shuffle<T>(this IList<T> list)
	{
		Random rnd = new Random();
		var copy = new List<T>(list);
		int n = copy.Count;
		while (n > 1)
		{
			n--;
			int k = rnd.Next(n + 1);
			T value = copy[k];
			copy[k] = copy[n];
			copy[n] = value;
		}

		return copy;
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