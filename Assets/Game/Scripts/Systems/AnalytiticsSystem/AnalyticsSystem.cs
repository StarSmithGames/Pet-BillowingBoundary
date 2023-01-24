using Game.Managers.StorageManager;
using Game.Systems.AdSystem;
using System.Collections.Generic;

namespace Game.Systems.AnalyticsSystem
{
	public class AnalyticsSystem
	{
		private AmplitudeAnalyticsGroup amplitudeAnalyticsGroup;
		private FirebaseAnalyticsGroup firebaseAnalyticsGroup;
		private UnityAnalyticsGroup unityAnalyticsGroup;
		private ISaveLoad saveLoad;

		public AnalyticsSystem(
			AmplitudeAnalyticsGroup amplitudeAnalyticsGroup,
			FirebaseAnalyticsGroup firebaseAnalyticsGroup,
			UnityAnalyticsGroup unityAnalyticsGroup,
			ISaveLoad saveLoad)
		{
			this.amplitudeAnalyticsGroup = amplitudeAnalyticsGroup;
			this.firebaseAnalyticsGroup = firebaseAnalyticsGroup;
			this.unityAnalyticsGroup = unityAnalyticsGroup;
			this.saveLoad = saveLoad;
		}

		#region Market
		public void LogEvent_bonus_upgraded(string name)
		{
			LogEvent($"bonus_upgraded_{name}");
		}

		public void LogEvent_skill_upgraded(string name)
		{
			LogEvent($"skill_upgraded_{name}");
		}
		#endregion

		#region IAP
		public void LogEvent_show_premium_market()
		{
			LogEvent("show_premium_market");
		}

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
		public void LogEvent_daily_reward_claimed()
		{
			LogEvent("daily_reward_claimed");
		}

		public void LogEvent_daily_reward_setup_next_day()
		{
			LogEvent("daily_reward_setup_next_day");
		}

		public void LogEvent_daily_reward_reseted()
		{
			LogEvent("daily_reward_reseted");
		}

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

		#region Settings
		public void LogEvent_settings_language(string lang)
		{
			LogEvent($"settings_language_{lang}");
		}

		public void LogEvent_settings_music()
		{
			LogEvent("settings_music");
		}

		public void LogEvent_settings_sound()
		{
			LogEvent("settings_sound");
		}

		public void LogEvent_settings_vibration()
		{
			LogEvent("settings_vibration");
		}
		#endregion

		public void LogEvent(string id, Dictionary<string, object> parameters = null)
		{
			id = saveLoad.GetStorage().IsPayUser.GetData() ? $"WHALE_{id}" : id;

			if (parameters == null)
			{
				amplitudeAnalyticsGroup.LogEvent(id);
				firebaseAnalyticsGroup.LogEvent(id);
				unityAnalyticsGroup.LogEvent(id);
			}
			else
			{
				amplitudeAnalyticsGroup.LogEvent(id, parameters);
				firebaseAnalyticsGroup.LogEvent(id, parameters);
				unityAnalyticsGroup.LogEvent(id, parameters);
			}
		}
	}
}