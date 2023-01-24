using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.AdSystem
{
	public class AdRewarded : IAdPlacement, IInitializable
	{
		public event UnityAction onRewardClosed;
		public event UnityAction onRewardedClosed;

		public bool IsEnabled { get; private set; } = true;
		public bool IsShowing { get; private set; } = false;

		private AnalyticsSystem.AnalyticsSystem analyticsSystem;

		public AdRewarded(AnalyticsSystem.AnalyticsSystem analyticsSystem)
		{
			this.analyticsSystem = analyticsSystem;
		}

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

			Debug.Log("[AdSystem] Rewarded Load!");
		}


		public void Enable(bool trigger)
		{
			IsEnabled = true;
		}

		public bool Show()
		{
			if (IronSource.Agent.isRewardedVideoAvailable())
			{
				IronSource.Agent.showRewardedVideo();

				Debug.Log("[AdSystem] Rewarded Show.");

				return true;
			}

			return false;
		}

		public void Hide() { }

		/************* RewardedVideo AdInfo Delegates *************/
		// Indicates that there’s an available ad.
		// The adInfo object includes information about the ad that was loaded successfully
		// This replaces the RewardedVideoAvailabilityChangedEvent(true) event
		private void OnRewardedAdAvailable(IronSourceAdInfo adInfo)
		{
		}

		// The Rewarded Video ad view has opened. Your activity will loose focus.
		private void OnRewardedAdOpened(IronSourceAdInfo adInfo)
		{
			IsShowing = true;
			analyticsSystem.LogEvent_ad_rewarded_showed();
		}
		// The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
		private void OnRewardedAdClosed(IronSourceAdInfo adInfo)
		{
			IronSource.Agent.loadRewardedVideo();

			analyticsSystem.LogEvent_ad_rewarded_closed(RewardedClosedType.Simple);

			IsShowing = false;

			onRewardClosed?.Invoke();
		}
		// The user completed to watch the video, and should be rewarded.
		// The placement parameter will include the reward data.
		// When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
		private void OnRewardedAdRewarded(IronSourcePlacement placement, IronSourceAdInfo adInfo)
		{
			analyticsSystem.LogEvent_ad_rewarded_closed(RewardedClosedType.Rewarded);
			onRewardedClosed?.Invoke();
		}

		// Invoked when the video ad was clicked.
		// This callback is not supported by all networks, and we recommend using it only if
		// it’s supported by all networks you included in your build.
		private void OnRewardedAdClicked(IronSourcePlacement placement, IronSourceAdInfo adInfo)
		{
			analyticsSystem.LogEvent_ad_rewarded_closed(RewardedClosedType.Clicked);
		}


		// Indicates that no ads are available to be displayed
		// This replaces the RewardedVideoAvailabilityChangedEvent(false) event
		private void OnRewardedAdUnavailable()
		{
			analyticsSystem.LogEvent_ad_rewarded_failed();

			Debug.LogError($"[AdSystem] Rewarded Unavailable.");
		}

		// The rewarded video ad was failed to show.
		private void OnRewardedAdShowFailed(IronSourceError error, IronSourceAdInfo adInfo)
		{
			analyticsSystem.LogEvent_ad_rewarded_failed();

			Debug.LogError($"[AdSystem] Rewarded Failed {error.getDescription()}");
		}
	}

	public enum RewardedClosedType
	{
		None,

		Simple,
		Clicked,
		Rewarded,
	}
}