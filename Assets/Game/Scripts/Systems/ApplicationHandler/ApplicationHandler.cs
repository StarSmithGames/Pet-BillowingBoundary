using Game.Managers.StorageManager;
using Game.Systems.WaveRoadSystem;

using UnityEngine;

using Zenject;

namespace Game.Systems.ApplicationHandler
{
	public class ApplicationHandler : MonoBehaviour
	{
		private SignalBus signalBus;

		[Inject]
		private void Construct(SignalBus signalBus, ISaveLoad saveLoad)
		{
			this.signalBus = signalBus;

			if (saveLoad.GetStorage().IsWasHere.GetData() == true)
			{
				saveLoad.GetStorage().IsFirstTime.SetData(false);
			}

			Application.runInBackground = true;
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