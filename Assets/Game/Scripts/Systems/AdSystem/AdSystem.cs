using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdSystem : MonoBehaviour
{
	private IEnumerator Start()
	{
		IronSource.Agent.validateIntegration();
		IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
		IronSource.Agent.init("17f0b51a5", IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);

		yield return new WaitForSeconds(1f);
		IronSource.Agent.loadInterstitial();
		IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
		yield return new WaitForSeconds(1f);
		IronSource.Agent.displayBanner();
	}

	public void ShowInterstitial()
	{
		IronSource.Agent.showInterstitial();
	}


	private void SdkInitializationCompletedEvent()
	{
		Debug.LogError("SdkInitializationCompletedEvent");
	}
}
