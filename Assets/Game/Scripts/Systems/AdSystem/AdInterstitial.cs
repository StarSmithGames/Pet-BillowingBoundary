using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.AdSystem
{
	public class AdInterstitial : IInitializable
	{
		public event UnityAction onInterstitialClosed;

		private bool isClicked = false;

		private AnalyticsSystem.AnalyticsSystem analyticsSystem;

		public AdInterstitial(AnalyticsSystem.AnalyticsSystem analyticsSystem)
		{
			this.analyticsSystem = analyticsSystem;
		}

		public void Initialize()
		{
			IronSourceInterstitialEvents.onAdReadyEvent += OnInterstitialAdReady;
			IronSourceInterstitialEvents.onAdLoadFailedEvent += OnInterstitialonAdLoadFailed;
			IronSourceInterstitialEvents.onAdOpenedEvent += OnInterstitialAdOpened;
			IronSourceInterstitialEvents.onAdClickedEvent += OnInterstitialAdClicked;
			IronSourceInterstitialEvents.onAdShowSucceededEvent += OnInterstitialAdShowSucceeded;
			IronSourceInterstitialEvents.onAdShowFailedEvent += OnInterstitialAdShowFailed;
			IronSourceInterstitialEvents.onAdClosedEvent += OnInterstitialAdClosed;

			IronSource.Agent.loadInterstitial();

			Debug.Log("[AdSystem] Interstitial Load!");
		}

		public bool Show()
		{
			if (IronSource.Agent.isInterstitialReady())
			{
				IronSource.Agent.showInterstitial();
				Debug.Log("[AdSystem] Interstitial Show.");
				return true;
			}

			return false;
		}

		/************* Interstitial AdInfo Delegates *************/
		// Invoked when the interstitial ad was loaded succesfully.
		private void OnInterstitialAdReady(IronSourceAdInfo adInfo)
		{
		}

		// Invoked before the interstitial ad was opened, and before the InterstitialOnAdOpenedEvent is reported.
		// This callback is not supported by all networks, and we recommend using it only if  
		// it's supported by all networks you included in your build. 
		private void OnInterstitialAdShowSucceeded(IronSourceAdInfo adInfo)
		{
			analyticsSystem.LogEvent_ad_interstitial_showed();
		}

		// Invoked when the Interstitial Ad Unit has opened. This is the impression indication. 
		private void OnInterstitialAdOpened(IronSourceAdInfo adInfo)
		{
		}

		// Invoked when end user clicked on the interstitial ad
		private void OnInterstitialAdClicked(IronSourceAdInfo adInfo)
		{
			isClicked = true;
		}

		// Invoked when the interstitial ad closed and the user went back to the application screen.
		private void OnInterstitialAdClosed(IronSourceAdInfo adInfo)
		{
			IronSource.Agent.loadInterstitial();
			Debug.Log("[AdSystem] Interstitial Load!");

			analyticsSystem.LogEvent_ad_interstitial_closed(isClicked ? InterstitialClosedType.Click : InterstitialClosedType.Simple);
			isClicked = false;

			onInterstitialClosed?.Invoke();
		}


		// Invoked when the ad failed to show.
		private void OnInterstitialAdShowFailed(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
		{
			analyticsSystem.LogEvent_ad_interstitial_failed();

			Debug.LogError($"[AdSystem] Interstitial Failed {ironSourceError.getDescription()}");
		}

		// Invoked when the initialization process has failed.
		private void OnInterstitialonAdLoadFailed(IronSourceError ironSourceError)
		{
			analyticsSystem.LogEvent_ad_interstitial_failed();

			Debug.LogError($"[AdSystem] Interstitial Failed {ironSourceError.getDescription()}");
		}
	}

	public enum InterstitialClosedType
	{
		Simple,
		Click,
	}
}