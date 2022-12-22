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

		private void OnApplicationFocus(bool focus)
		{
			
		}

		private void OnApplicationPause(bool pause)
		{
			
		}

		private void OnApplicationQuit()
		{
			signalBus?.Fire(new SignalApplicationQuit());
		}
	}
}