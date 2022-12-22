using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;

namespace Game.Managers.StorageManager
{
	public class Database
	{
		private Dictionary<string, object> Data = new Dictionary<string, object>();

		public bool IsHas(string key)
		{
			return Data.ContainsKey(key);
		}

		public T Get<T>(string key, T defaultValue = default)
		{
			if (Data.ContainsKey(key))
			{
				try
				{
					object data = Data[key];

					if (data is JObject jdata)// >:c
					{
						data = jdata.ToObject<T>();
						Data[key] = data;
					}

					return (T)data;
				}
				catch (InvalidCastException)
				{
					return defaultValue;
				}
			}
			return defaultValue;
		}

		public void Set<T>(string key, T value)
		{
			Data[key] = value;
		}

		public Dictionary<string, object> GetDictionary() => Data;

		public string GetJson()
		{
			return JsonSerializator.SerializeObjectToJson(Data);
		}
		public void LoadJson(string json)
		{
			Data = JsonSerializator.DeserializeObjectFromJson(json);
		}

		public void Drop()
		{
			Data = new Dictionary<string, object>();
		}
	}
}