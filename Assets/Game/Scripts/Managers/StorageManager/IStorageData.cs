using Game.Systems.DailyRewardSystem;
using Game.Systems.LocalizationSystem;

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

		public IStorageData<bool> IsFirstTime { get; private set; }

		public IStorageData<LocalizationSystem.Data> LocalizationData { get; private set; }
		public IStorageData<DailyRewardSystem.Data> DailyRewardData { get; private set; }

		//public bool IsHasProfile => CurrentProfile.GetData() != null;//like IsFirstTime
		//public IStorageData<Profile> CurrentProfile { get; private set; }

		/// <summary>
		/// Default Data
		/// </summary>
		public Storage()
		{
			Database = new Database();

			Initialization();
		}

		/// <summary>
		/// Json Data
		/// </summary>
		public Storage(string json)
		{
			Database = new Database();
			Database.LoadJson(json);

			Initialization();
		}

		public void Clear()
		{
			Database.Drop();
			Initialization();
		}

		private void Initialization()
		{
			IsFirstTime = new StorageData<bool>(Database, "is_first_time", true);

			LocalizationData = new StorageData<LocalizationSystem.Data>(Database, "localization_data", new LocalizationSystem.Data());
			DailyRewardData = new StorageData<DailyRewardSystem.Data>(Database, "daily_reward_data", new DailyRewardSystem.Data());
			//CurrentProfile = new StorageData<Profile>(Database, "profile_current", new Profile());
		}

		[System.Serializable]
		public class Reference
		{
			public string displayName;
			public string fileName;
		}
	}

	public class Profile
	{

	}
}