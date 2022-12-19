using System.Collections.Generic;

public static class CollectionExtensions
{
	public static T RandomItem<T>(this IList<T> list, int from = 0, int to = -1)
	{
		if (list.Count == 0) return default;
		return list[UnityEngine.Random.Range(from, to == -1 ? list.Count : to)];
	}
}