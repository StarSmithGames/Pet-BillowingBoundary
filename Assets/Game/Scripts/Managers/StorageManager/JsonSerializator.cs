using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

using Sirenix.Serialization;
using UnityEditor;

namespace Game.Managers.StorageManager
{
	public class JsonSerializator : MonoBehaviour
	{
		#region PlayerPrefs
		public static void SaveToPlayerPrefs(string key, string data)
		{
			PlayerPrefs.SetString(key, data);
		}

		public static string LoadFromPlayerPrefs(string key)
		{
			return PlayerPrefs.GetString(key);
		}
		#endregion

		#region Unity Serialiation
		public static string ConvertToUnityJson(object data)
		{
			return JsonUtility.ToJson(data, true);
		}

		public static T ConvertFromUnityJson<T>(string json)
		{
			return JsonUtility.FromJson<T>(json);
		}
		#endregion

		//public static string ConvertToOdinJson(object data)
		//{
		//	var json = Sirenix.Serialization.SerializationUtility.SerializeValue(data, DataFormat.JSON);
		//	string tmp = System.Convert.ToBase64String(json);
		//	return tmp;
		//}

		//public static byte[] ConvertToOdinByteJson(object data)
		//{
		//	return Sirenix.Serialization.SerializationUtility.SerializeValue(data, DataFormat.JSON);
		//}

		//public static T ConvertFromOdinJson<T>(string json)
		//{
		//	byte[] tmp = System.Convert.FromBase64String(json);
		//	return ConvertBytesOdinObject<T>(tmp);
		//}

		//public static T ConvertBytesOdinObject<T>(byte[] bytes)
		//{
		//	return Sirenix.Serialization.SerializationUtility.DeserializeValue<T>(bytes, DataFormat.JSON);
		//}

		#region Complex Dictionary<string, object> Serialization
		public static string SerializeObjectToJson(Dictionary<string, object> data)
		{
			return JsonConvert.SerializeObject(data,
			new JsonSerializerSettings()
			{
				Formatting = Formatting.Indented,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				TypeNameHandling = TypeNameHandling.Auto,
			});
		}
		public static Dictionary<string, object> DeserializeObjectFromJson(string json)
		{
			return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
		}
		#endregion

		#region Application.persistentDataPath
		public static void SaveBytesToFile(byte[] bytes, string filepath)
		{
			string dir = Application.persistentDataPath + "/Data/";

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			File.WriteAllBytes(dir + filepath, bytes);
		}

		public static byte[] LoadBytesFromFile(string filepath)
		{
			string dir = Application.persistentDataPath + "/Data/";
			return File.ReadAllBytes(dir + filepath);
		}

		public static void ClearPersistentPath()
		{
			foreach (var directory in Directory.GetDirectories(Application.persistentDataPath))
			{
				DirectoryInfo data_dir = new DirectoryInfo(directory);
				data_dir.Delete(true);
			}

			foreach (var file in Directory.GetFiles(Application.persistentDataPath))
			{
				FileInfo file_info = new FileInfo(file);
				file_info.Delete();
			}
		}
		#endregion


		private static void SaveDataToJson<T>(T data, string directory, string fileName)
		{
			string dir = Application.persistentDataPath + directory;

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			string jsonData = JsonUtility.ToJson(data, true);
			File.WriteAllText(dir + fileName, jsonData);
		}

		private static T LoadDataFromJson<T>(string directory, string fileName)
		{
			string fullPath = Application.persistentDataPath + directory + fileName;

			if (File.Exists(fullPath))
			{
				string json = File.ReadAllText(fullPath);
				return JsonUtility.FromJson<T>(json);
			}

			throw new Exception("File doesn't exist");
		}
		private static T LoadDataFromJson<T>(string fullPath)
		{
			if (File.Exists(fullPath))
			{
				string json = File.ReadAllText(fullPath);
				return JsonUtility.FromJson<T>(json);
			}

			throw new Exception("File doesn't exist");
		}
	}
}