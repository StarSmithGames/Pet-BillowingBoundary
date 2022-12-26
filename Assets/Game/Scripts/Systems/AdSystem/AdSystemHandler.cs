using System;
using UnityEngine;

using Zenject;

namespace Game.Systems.AdSystem
{
	public class AdSystemHandler : IInitializable, ITickable, IDisposable
	{
		private float duration = 120f;//2 minutes
		private float duration2 = 300f;//5 minutes
		private float t = 0;
		private bool isInterstitial = false;

		private AdSystem adSystem;

		public AdSystemHandler(AdSystem adSystem)
		{
			this.adSystem = adSystem;
		}

		public void Initialize()
		{
			adSystem.AdInterstitial.onInterstitialClosed += OnInterstitialClosed;
		}

		public void Dispose()
		{
			if(adSystem != null)
			{
				adSystem.AdInterstitial.onInterstitialClosed -= OnInterstitialClosed;
			}
		}

		public void Tick()
		{
			if (!isInterstitial)
			{
				t += Time.deltaTime;

				if (t >= duration)
				{
					if (adSystem.AdInterstitial.Show())
					{
						isInterstitial = true;
						t = 0;
					}
				}
			}
		}

		private void OnInterstitialClosed()
		{
			isInterstitial = false;
		}
	}
}