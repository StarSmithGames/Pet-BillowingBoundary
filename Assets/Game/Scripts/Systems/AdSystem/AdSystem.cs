using Game.Systems.ApplicationHandler;

using UnityEngine;

using Zenject;

namespace Game.Systems.AdSystem
{
	public class AdSystem
	{
		public AdBanner AdBanner { get; private set; }
		public AdInterstitial AdInterstitial { get; private set; }
		public AdRewarded AdRewarded { get; private set; }

		private SignalBus signalBus;

		public AdSystem(SignalBus signalBus, string appId, AdBanner adBanner, AdInterstitial adInterstitial, AdRewarded adRewarded)
		{
			this.signalBus = signalBus;

			AdBanner = adBanner;
			AdInterstitial = adInterstitial;
			AdRewarded = adRewarded;

			IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
			IronSource.Agent.init(appId, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);

			signalBus?.Subscribe<SignalApplicationPause>(OnApplicationPaused);

			Debug.Log("[AdSystem] Initialization!");
		}

		private void SdkInitializationCompletedEvent()
		{
			Debug.Log("[AdSystem] Initialization Completed!");
		}

		private void OnApplicationPaused(SignalApplicationPause signal)
		{
			IronSource.Agent.onApplicationPause(signal.trigger);
		}
	}
}