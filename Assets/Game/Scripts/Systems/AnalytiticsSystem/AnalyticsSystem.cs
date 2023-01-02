using Game.Managers.StorageManager;
using Game.Systems.AdSystem;

using System.Collections.Generic;

namespace Game.Systems.AnalyticsSystem
{
	public class AnalyticsSystem
	{

		private FirebaseAnalyticsGroup firebaseAnalyticsGroup;
		private ISaveLoad saveLoad;

		public AnalyticsSystem(FirebaseAnalyticsGroup firebaseAnalyticsGroup, ISaveLoad saveLoad)
		{
			this.firebaseAnalyticsGroup = firebaseAnalyticsGroup;
			this.saveLoad = saveLoad;
		}

		#region Market

		#endregion

		#region IAP
		public void LogEvent_iap_remove_ads()
		{
			LogEvent("iap_remove_ads");
		}

		public void LogEvent_iap_free_mode()
		{
			LogEvent("iap_free_mode");
		}

		public void LogEvent_iap_buy(int number)
		{
			LogEvent($"iap_{number}");
		}
		#endregion

		#region ADS
		public void LogEvent_ad_banner_showed()
		{
			LogEvent("ad_banner_showed");
		}

		public void LogEvent_ad_banner_clicked()
		{
			LogEvent("ad_banner_clicked");
		}

		public void LogEvent_ad_banner_failed()
		{
			LogEvent("ad_banner_failed");
		}

		public void LogEvent_ad_interstitial_showed()
		{
			LogEvent("ad_interstitial_showed");
		}

		public void LogEvent_ad_interstitial_closed(InterstitialClosedType closedType)
		{
			LogEvent("ad_interstitial_closed", new Dictionary<string, object>() { { "content_type", closedType.ToString() } });
		}

		public void LogEvent_ad_interstitial_failed()
		{
			LogEvent("ad_interstitial_failed");
		}

		public void LogEvent_ad_rewarded_showed()
		{
			LogEvent("ad_rewarded_showed");
		}

		public void LogEvent_ad_rewarded_closed(RewardedClosedType closedType)
		{
			LogEvent("ad_rewarded_closed", new Dictionary<string, object>() { { "content_type", closedType.ToString() } });
		}

		public void LogEvent_ad_rewarded_failed()
		{
			LogEvent("ad_rewarded_failed");
		}
		#endregion

		#region Statistics
		public void LogEvent_achievment_get(string id)
		{
			LogEvent($"achievment_get_{id}");
		}

		public void LogEvent_wave_completed()
		{
			LogEvent("wave_completed");
		}

		public void LogEvent_target_defeat()
		{
			LogEvent("target_defeat");
		}

		public void LogEvent_boss_defeat()
		{
			LogEvent("boss_defeat");
		}
		#endregion

		public void LogEvent(string id, Dictionary<string, object> parameters = null)
		{
			if(parameters == null)
			{
				firebaseAnalyticsGroup.LogEvent(id);
			}
			else
			{
				firebaseAnalyticsGroup.LogEvent(id, parameters);
			}
		}
	}
}