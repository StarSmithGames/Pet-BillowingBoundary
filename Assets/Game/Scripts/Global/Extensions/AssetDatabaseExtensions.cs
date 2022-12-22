#if UNITY_EDITOR
using UnityEngine;
using System.Linq;
using UnityEditor;
#endif

public class AssetDatabaseExtensions
{
#if UNITY_EDITOR

	public static T LoadAsset<T>() where T : ScriptableObject
	{
		return LoadAssets<T>().FirstOrDefault();
	}

	public static T[] LoadAssets<T>(bool orderByName = true) where T : ScriptableObject
	{
		var result = AssetDatabase
			.FindAssets($"t:{typeof(T).Name}")
			.Select((guid) => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)));

		return orderByName ? result.OrderBy((x) => x.name).ToArray() : result.ToArray();
	}
#endif
}