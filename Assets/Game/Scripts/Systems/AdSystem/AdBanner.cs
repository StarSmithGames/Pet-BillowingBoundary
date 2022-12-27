using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.AdSystem
{
	public class AdBanner : IInitializable
	{
		public event UnityAction onBannerShowed;
		public event UnityAction onBannerHided;

		private AnalyticsSystem.AnalyticsSystem analyticsSystem;

		public AdBanner(AnalyticsSystem.AnalyticsSystem analyticsSystem)
		{
			this.analyticsSystem = analyticsSystem;
		}

		public void Initialize()
		{
			IronSourceEvents.onBannerAdLoadedEvent += OnBannerAdLoaded;
			IronSourceEvents.onBannerAdLoadFailedEvent += OnBannerAdLoadFailed;
			IronSourceEvents.onBannerAdClickedEvent += OnBannerAdClicked;
			IronSourceEvents.onBannerAdScreenPresentedEvent += OnBannerAdScreenPresented;
			IronSourceEvents.onBannerAdScreenDismissedEvent += OnBannerAdScreenDismissed;
			IronSourceEvents.onBannerAdLeftApplicationEvent += OnBannerAdLeftApplication;

			IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);

			Debug.Log("[AdSystem] Banner Load!");
		}

		//Invoked once the banner has loaded
		private void OnBannerAdLoaded()
		{
			IronSource.Agent.displayBanner();

			analyticsSystem.LogEvent_ad_banner_showed();

			onBannerShowed?.Invoke();
			//IronSource.Agent.hideBanner();
			//IronSource.Agent.destroyBanner();
		}

		// Invoked when end user clicks on the banner ad
		private void OnBannerAdClicked()
		{
			analyticsSystem.LogEvent_ad_banner_clicked();
		}

		//Notifies the presentation of a full screen content following user click
		private void OnBannerAdScreenPresented()
		{
		}

		//Notifies the presented screen has been dismissed
		private void OnBannerAdScreenDismissed()
		{
		}

		//Invoked when the user leaves the app
		private void OnBannerAdLeftApplication()
		{
		}

		//Invoked when the banner loading process has failed.
		//@param description - string - contains information about the failure.
		private void OnBannerAdLoadFailed(IronSourceError error)
		{
			analyticsSystem.LogEvent_ad_banner_failed();

			Debug.Log($"[AdSystem] Banner Failed {error.getDescription()}");
		}
	}
}