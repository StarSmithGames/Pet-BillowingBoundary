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

			Debug.LogError("Interstitial Load");
		}

		public bool Show()
		{
			if (IronSource.Agent.isInterstitialReady())
			{
				IronSource.Agent.showInterstitial();

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
		}

		// Invoked when the Interstitial Ad Unit has opened. This is the impression indication. 
		private void OnInterstitialAdOpened(IronSourceAdInfo adInfo)
		{
		}

		// Invoked when end user clicked on the interstitial ad
		private void OnInterstitialAdClicked(IronSourceAdInfo adInfo)
		{
		}

		// Invoked when the interstitial ad closed and the user went back to the application screen.
		private void OnInterstitialAdClosed(IronSourceAdInfo adInfo)
		{
			IronSource.Agent.loadInterstitial();

			onInterstitialClosed?.Invoke();
		}


		// Invoked when the ad failed to show.
		private void OnInterstitialAdShowFailed(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
		{
		}

		// Invoked when the initialization process has failed.
		private void OnInterstitialonAdLoadFailed(IronSourceError ironSourceError)
		{
		}
	}
}