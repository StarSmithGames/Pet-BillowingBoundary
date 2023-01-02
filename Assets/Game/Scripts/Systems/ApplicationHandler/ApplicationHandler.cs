using Game.Managers.StorageManager;

using UnityEngine;

using Zenject;

namespace Game.Systems.ApplicationHandler
{
	public class ApplicationHandler : MonoBehaviour
	{
		private SignalBus signalBus;
		private ISaveLoad saveLoad;

		[Inject]
		private void Construct(SignalBus signalBus, ISaveLoad saveLoad)
		{
			this.signalBus = signalBus;
			this.saveLoad = saveLoad;

			Application.runInBackground = true;

			if (saveLoad.GetStorage().IsWasHere.GetData() == true)
			{
				saveLoad.GetStorage().IsFirstTime.SetData(false);
			}
		}

		private void OnDestroy()
		{
			signalBus?.Fire(new SignalSave());
		}

		private void OnApplicationFocus(bool focus)
		{
			signalBus?.Fire(new SignalSave());
			signalBus?.Fire(new SignalApplicationFocus() { trigger = focus });
		}

		private void OnApplicationPause(bool pause)
		{
			signalBus?.Fire(new SignalSave());
			signalBus?.Fire(new SignalApplicationPause() { trigger = pause });
		}

		private void OnApplicationQuit()
		{
			signalBus?.Fire(new SignalSave());
			signalBus?.Fire(new SignalApplicationQuit());
		}
	}
}