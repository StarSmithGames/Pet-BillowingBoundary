using Game.Systems.ApplicationHandler;

using System;
using System.Collections.Generic;

using UnityEngine;
using Zenject;

namespace Game.Managers.StorageManager
{
	public interface ISaveLoad
	{
		void Save();

		/// <summary>
		/// Get currently selected storage 
		/// </summary>
		/// <returns>Current active storage.</returns>
		Storage GetStorage();
	}

	public class PlayerPrefsSaveLoad : ISaveLoad, IInitializable, IDisposable
	{
		private Storage activeStorage;

		private SignalBus signalBus;
		private Settings settings;

		public PlayerPrefsSaveLoad(SignalBus signalBus, Settings settings)
		{
			this.signalBus = signalBus;
			this.settings = settings;
		}

		public void Initialize()
		{
			signalBus?.Subscribe<SignalApplicationRequiredSave>(OnApplicationRequiredSave);

			if (activeStorage == null)
			{
				Load();
			}
		}

		public void Dispose()
		{
			signalBus?.Unsubscribe<SignalApplicationRequiredSave>(OnApplicationRequiredSave);

			Save();
		}

		public void Save()
		{
			signalBus?.Fire(new SignalSaveData());

			PlayerPrefs.SetString(settings.profileName, activeStorage.Profile.GetJson());
			PlayerPrefs.SetString(settings.dataName, activeStorage.Database.GetJson());
			PlayerPrefs.Save();

			Debug.Log($"[PlayerPrefsSaveLoad] Save storage to pref");
		}

		public void Load()
		{
			if (PlayerPrefs.HasKey(settings.dataName))
			{
				string profile = PlayerPrefs.GetString(settings.profileName);
				string data = PlayerPrefs.GetString(settings.dataName);

				activeStorage = new Storage(profile, data);
			}
			else//first time
			{
				activeStorage = new Storage();

				Debug.Log($"[PlayerPrefsSaveLoad] Create new save");

				Save();
			}

			Debug.Log($"[PlayerPrefsSaveLoad] Load storage from pref: {settings.dataName}");
		}

		public Storage GetStorage()
		{
			if (activeStorage == null)
			{
				Load();
			}

			return activeStorage;
		}

		private void OnApplicationRequiredSave()
		{
			if(GetStorage().IsFirstTime.GetData() == true)
			{
				GetStorage().IsFirstTime.SetData(false);
			}

			Save();
		}

		[System.Serializable]
		public class Settings
		{
			public string profileName = "profile";
			public string dataName = "save_data";
			//public string storageFileName = "Profile.dat";

			public List<string> GetKeyList()
			{
				return new List<string>()
				{
					profileName,
					dataName,
				};
			}
		}
	}
}