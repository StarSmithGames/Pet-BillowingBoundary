using Game.Entities;
using Game.Systems.WaveRoadSystem;

using System.Collections.Generic;

using UnityEngine;

namespace Game.Managers.StorageManager
{
	public interface IStorageData<T>
	{
		T GetData();
		void SetData(T data);
	}

	public class StorageData<T> : IStorageData<T>
	{
		private Database database;
		private string key;
		private T defaultValue;

		public StorageData(Database database, string key, T defaultValue = default)
		{
			this.database = database;
			this.key = key;
			this.defaultValue = defaultValue;

			SetData(database.Get(GetDataKey(), defaultValue));
		}

		public string GetDataKey()
		{
			return key + "_key";
		}
		public T GetData()
		{
			return database.Get<T>(GetDataKey(), defaultValue);
		}
		public void SetData(T data)
		{
			database.Set(GetDataKey(), data);
		}
	}

	public class Storage
	{
		public Database Database { get; private set; }

		public Profile Profile { get; private set; }

		public IStorageData<bool> IsWasHere{ get; private set; }
		public IStorageData<bool> IsFirstTime { get; private set; }

		public IStorageData<bool> IsPayUser { get; private set; }
		public IStorageData<bool> IsBuyRemoveADS { get; private set; }
		public IStorageData<bool> IsBuyFreeMode { get; private set; }

		public IStorageData<bool> IsCompleteTutorial { get; private set; }

		public IStorageData<FireFistSkill.Data> FireFistSkill { get; private set; }

		//settings
		public IStorageData<bool> IsSound { get; private set; }
		public IStorageData<bool> IsMusic { get; private set; }
		public IStorageData<bool> IsVibration { get; private set; }
		public IStorageData<int> LanguageIndex { get; private set; }


		/// <summary>
		/// Default Data
		/// </summary>
		public Storage()
		{
			Profile = new Profile();
			Database = new Database();

			Initialization();
		}

		/// <summary>
		/// Json Data
		/// </summary>
		public Storage(string profile, string data)
		{
			Profile = new Profile(profile);
			Database = new Database();
			Database.LoadJson(data);

			Initialization();
		}

		public void Clear()
		{
			Database.Drop();
			Initialization();
		}

		private void Initialization()
		{
			IsWasHere = new StorageData<bool>(Database, "is_was_here", false);
			IsFirstTime = new StorageData<bool>(Database, "is_first_time", true);

			IsPayUser = new StorageData<bool>(Database, "is_pay_user", false);
			IsBuyRemoveADS = new StorageData<bool>(Database, "is_buy_remove_ads", false);
			IsBuyFreeMode = new StorageData<bool>(Database, "is_buy_free_mode", false);

			IsCompleteTutorial = new StorageData<bool>(Database, "tutorial_tap", false);

			FireFistSkill = new StorageData<FireFistSkill.Data>(Database, "fire_fist_skill", new FireFistSkill.Data());

			IsSound = new StorageData<bool>(Database, "is_sound", true);
			IsMusic = new StorageData<bool>(Database, "is_music", true);
			IsVibration = new StorageData<bool>(Database, "is_vibration", true);
			LanguageIndex = new StorageData<int>(Database, "language_index", 0);
		}

		[System.Serializable]
		public class Reference
		{
			public string displayName;
			public string fileName;
		}
	}

	[System.Serializable]
	public class Profile
	{
		public Data data;

		public Profile()
		{
			data = new Data();
		}

		public Profile(string json)
		{
			data = JsonSerializator.ConvertFromJson<Data>(json);
		}

		public string GetJson()
		{
			return JsonSerializator.ConvertToJson(data);
		}

		public string GetDefaultJson()
		{
			return JsonSerializator.ConvertToJson(new Data());
		}

		public Data GetData()
		{
			return data;
		}

		[System.Serializable]
		public class Data
		{
			public PlayerData playerData;
			public WaveRoad.Data waveRoadData;
		}
	}
}