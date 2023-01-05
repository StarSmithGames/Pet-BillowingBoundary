using Game.Managers.StorageManager;
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
		private ISaveLoad saveLoad;

		public AdSystem(
			SignalBus signalBus,
			string appId,
			bool isDebug,
			AdBanner adBanner,
			AdInterstitial adInterstitial,
			AdRewarded adRewarded,
			ISaveLoad saveLoad)
		{
			this.signalBus = signalBus;
			this.saveLoad = saveLoad;

			AdBanner = adBanner;
			AdInterstitial = adInterstitial;
			AdRewarded = adRewarded;

			IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
			IronSource.Agent.init(appId, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);

			RefreshAd();

			signalBus?.Subscribe<SignalApplicationPause>(OnApplicationPaused);

			if (isDebug)
			{
				AdBanner.Enable(false);
				AdInterstitial.Enable(false);
			}

			Debug.Log("[AdSystem] Initialization!");
		}

		public void Enable(bool trigger)
		{
			RefreshAd();

			signalBus?.Fire(new SignalADSEnableChanged() { trigger = trigger });
		}

		private void RefreshAd()
		{
			bool isRemoveADS = saveLoad.GetStorage().IsBuyRemoveADS.GetData();
			AdBanner.Enable(!isRemoveADS);
			AdInterstitial.Enable(!isRemoveADS);
			//AdRewarded.Enable(!isRemoveADS);
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

	public interface IAdPlacement
	{
		bool IsEnabled { get; }
		bool IsShowing { get; }

		void Enable(bool trigger);

		bool Show();
		void Hide();
	}
}