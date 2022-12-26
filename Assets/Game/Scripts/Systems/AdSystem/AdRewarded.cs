using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Systems.AdSystem
{
	public class AdRewarded : IInitializable
	{
		public void Initialize()
		{
			IronSourceRewardedVideoEvents.onAdOpenedEvent += OnRewardedAdOpened;
			IronSourceRewardedVideoEvents.onAdClosedEvent += OnRewardedAdClosed;
			IronSourceRewardedVideoEvents.onAdAvailableEvent += OnRewardedAdAvailable;
			IronSourceRewardedVideoEvents.onAdUnavailableEvent += OnRewardedAdUnavailable;
			IronSourceRewardedVideoEvents.onAdShowFailedEvent += OnRewardedAdShowFailed;
			IronSourceRewardedVideoEvents.onAdRewardedEvent += OnRewardedAdRewarded;
			IronSourceRewardedVideoEvents.onAdClickedEvent += OnRewardedAdClicked;

			IronSource.Agent.loadRewardedVideo();

			Debug.LogError("Rewarded Load");
		}

		public bool Show()
		{
			if (IronSource.Agent.isRewardedVideoAvailable())
			{
				IronSource.Agent.showRewardedVideo();
				
				return true;
			}

			return false;
		}

		/************* RewardedVideo AdInfo Delegates *************/
		// Indicates that there’s an available ad.
		// The adInfo object includes information about the ad that was loaded successfully
		// This replaces the RewardedVideoAvailabilityChangedEvent(true) event
		void OnRewardedAdAvailable(IronSourceAdInfo adInfo)
		{
		}

		// The Rewarded Video ad view has opened. Your activity will loose focus.
		void OnRewardedAdOpened(IronSourceAdInfo adInfo)
		{
		}
		// The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
		void OnRewardedAdClosed(IronSourceAdInfo adInfo)
		{
			IronSource.Agent.loadRewardedVideo();
		}
		// The user completed to watch the video, and should be rewarded.
		// The placement parameter will include the reward data.
		// When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
		void OnRewardedAdRewarded(IronSourcePlacement placement, IronSourceAdInfo adInfo)
		{
		}

		// Invoked when the video ad was clicked.
		// This callback is not supported by all networks, and we recommend using it only if
		// it’s supported by all networks you included in your build.
		void OnRewardedAdClicked(IronSourcePlacement placement, IronSourceAdInfo adInfo)
		{
		}


		// Indicates that no ads are available to be displayed
		// This replaces the RewardedVideoAvailabilityChangedEvent(false) event
		void OnRewardedAdUnavailable()
		{
		}

		// The rewarded video ad was failed to show.
		void OnRewardedAdShowFailed(IronSourceError error, IronSourceAdInfo adInfo)
		{

		}
	}
}