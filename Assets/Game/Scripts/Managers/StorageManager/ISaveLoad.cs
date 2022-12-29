using Game.Systems.ApplicationHandler;

using System;
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
			signalBus?.Subscribe<SignalApplicationQuit>(OnApplicationQuit);
			signalBus?.Subscribe<SignalApplicationPause>(OnApplicationPaused);
			signalBus?.Subscribe<SignalApplicationFocus>(OnApplicationFocusChanged);

			if (activeStorage == null)
			{
				Load();
			}
		}

		public void Dispose()
		{
			signalBus?.Unsubscribe<SignalApplicationQuit>(OnApplicationQuit);
			signalBus?.Unsubscribe<SignalApplicationPause>(OnApplicationPaused);
			signalBus?.Unsubscribe<SignalApplicationFocus>(OnApplicationFocusChanged);

			Save();
		}

		public void Save()
		{
			signalBus?.Fire(new SignalSaveData());

			string preferenceName = settings.preferenceName;
			PlayerPrefs.SetString(preferenceName, activeStorage.Database.GetJson());
			PlayerPrefs.Save();

			Debug.Log($"[PlayerPrefsSaveLoad] Save storage to pref: {preferenceName}");
		}

		public void Load()
		{
			if (PlayerPrefs.HasKey(settings.preferenceName))
			{
				string json = PlayerPrefs.GetString(settings.preferenceName);

				activeStorage = new Storage(json);
			}
			else//first time
			{
				activeStorage = new Storage();

				Debug.Log($"[PlayerPrefsSaveLoad] Create new save");

				Save();
			}

			Debug.Log($"[PlayerPrefsSaveLoad] Load storage from pref: {settings.preferenceName}");
		}

		public Storage GetStorage()
		{
			if (activeStorage == null)
			{
				Load();
			}

			return activeStorage;
		}

		private void OnApplicationFocusChanged(SignalApplicationFocus signal)
		{
			Save();
		}

		private void OnApplicationPaused(SignalApplicationPause signal)
		{
			Save();
		}

		private void OnApplicationQuit()
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
			public string preferenceName = "save_data";
			public string storageDisplayName = "Profile";
			public string storageFileName = "Profile.dat";
		}
	}
}