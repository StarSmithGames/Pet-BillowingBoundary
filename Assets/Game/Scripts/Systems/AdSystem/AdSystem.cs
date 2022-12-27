using UnityEngine;

using Zenject;

namespace Game.Systems.AdSystem
{
	public class AdSystem
	{
		public AdBanner AdBanner { get; private set; }
		public AdInterstitial AdInterstitial { get; private set; }
		public AdRewarded AdRewarded { get; private set; }

		public AdSystem(string appId, AdBanner adBanner, AdInterstitial adInterstitial, AdRewarded adRewarded)
		{
			AdBanner = adBanner;
			AdInterstitial = adInterstitial;
			AdRewarded = adRewarded;

			IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
			IronSource.Agent.init(appId, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);

			Debug.Log("[AdSystem] Initialization!");
		}

		private void SdkInitializationCompletedEvent()
		{
			Debug.Log("[AdSystem] Initialization Completed!");
		}
	}
}