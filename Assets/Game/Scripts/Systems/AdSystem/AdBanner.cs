using Game.Managers.AsyncManager;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Game.Systems.AdSystem
{
	public class AdBanner : IInitializable
	{
		public void Initialize()
		{
			IronSourceEvents.onBannerAdLoadedEvent += OnBannerAdLoaded;
			IronSourceEvents.onBannerAdLoadFailedEvent += OnBannerAdLoadFailed;
			IronSourceEvents.onBannerAdClickedEvent += OnBannerAdClicked;
			IronSourceEvents.onBannerAdScreenPresentedEvent += OnBannerAdScreenPresented;
			IronSourceEvents.onBannerAdScreenDismissedEvent += OnBannerAdScreenDismissed;
			IronSourceEvents.onBannerAdLeftApplicationEvent += OnBannerAdLeftApplication;

			IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);

		}

		//Invoked once the banner has loaded
		private void OnBannerAdLoaded()
		{
			IronSource.Agent.displayBanner();
			//IronSource.Agent.hideBanner();
			//IronSource.Agent.destroyBanner();
		}

		// Invoked when end user clicks on the banner ad
		private void OnBannerAdClicked()
		{
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
		}
	}
}