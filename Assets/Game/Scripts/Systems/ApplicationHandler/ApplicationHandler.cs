using UnityEngine;

using Zenject;

namespace Game.Systems.ApplicationHandler
{
	public class ApplicationHandler : MonoBehaviour
	{
		private SignalBus signalBus;

		[Inject]
		private void Construct(SignalBus signalBus)
		{
			this.signalBus = signalBus;
		}

		private void Start()
		{
			Application.runInBackground = true;

			signalBus?.Fire(new SignalApplicationRequiredSave());
		}

		private void OnDestroy()
		{
			signalBus?.Fire(new SignalApplicationRequiredSave());
		}

		private void OnEnable()
		{
			signalBus?.Fire(new SignalApplicationRequiredSave());
		}

		private void OnDisable()
		{
			signalBus?.Fire(new SignalApplicationRequiredSave());
		}

		private void OnApplicationFocus(bool focus)
		{
			signalBus?.Fire(new SignalApplicationRequiredSave());
			signalBus?.Fire(new SignalApplicationFocus() { trigger = focus });
		}

		private void OnApplicationPause(bool pause)
		{
			signalBus?.Fire(new SignalApplicationRequiredSave());
			signalBus?.Fire(new SignalApplicationPause() { trigger = pause });
		}

		private void OnApplicationQuit()
		{
			signalBus?.Fire(new SignalApplicationRequiredSave());
			signalBus?.Fire(new SignalApplicationQuit());
		}
	}
}