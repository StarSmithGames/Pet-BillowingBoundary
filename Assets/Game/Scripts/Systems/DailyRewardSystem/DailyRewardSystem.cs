using Game.Managers.NetworkTimeManager;
using Game.Managers.StorageManager;
using Game.UI;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.DailyRewardSystem
{
    public class DailyRewardSystem : IInitializable, ITickable
    {
		public UnityAction onChanged;

		private bool isInitialized = false;
		private Data data;
		private TimeSpan oneDay = TimeSpan.FromDays(1f);
		private float checkDuration = 5f;
		private float t = 0;
		private bool needCheck = false;

		private SignalBus signalBus;
		private DailyRewardSetting setting;
		private UISubCanvas subCanvas;
		private ISaveLoad saveLoad;
		private NetworkTimeManager networkTimeManager;
		private AnalyticsSystem.AnalyticsSystem analyticsSystem;

		public DailyRewardSystem(
			SignalBus signalBus,
			DailyRewardSetting setting,
			UISubCanvas subCanvas,
			ISaveLoad saveLoad,
			NetworkTimeManager networkTimeManager,
			AnalyticsSystem.AnalyticsSystem analyticsSystem)
        {
			this.signalBus = signalBus;
			this.setting = setting;
			this.subCanvas = subCanvas;
			this.saveLoad = saveLoad;
			this.networkTimeManager = networkTimeManager;
			this.analyticsSystem = analyticsSystem;
		}

		public void Initialize()
		{
			data = saveLoad.GetStorage().DailyRewardData.GetData();

			if (saveLoad.GetStorage().IsFirstTime.GetData() == true)//FirstTime
			{
				isInitialized = true;
				Reset();
			}
			else
			{
				//if (networkTimeManager.IsTrustedTime())//need server
				{
					isInitialized = true;
					if (IsMissedDay())
					{
						Reset();
					}
				}
			}
		}

		public void Tick()
		{
			if (!isInitialized) return;

			//if (data.currentState != DailyRewardState.Claimed)
			//{
			//	if (needCheck)
			//	{
			//		if (IsMissedDay())
			//		{
			//			Debug.LogError("Missed Day");
			//			Reset();
			//		}

			//		needCheck = false;
			//	}
			//	else
			//	{
			//		t += Time.deltaTime;

			//		if (t >= checkDuration)
			//		{
			//			t = 0;
			//			needCheck = true;
			//		}
			//	}
			//}
		}

		public List<DailyReward> GetRewards()
		{
			return setting.rewards;
		}


		public bool IsHasReward()
		{
			return !IsMissedDay() && IsCanClaimReward() || IsFirstDay();
		}

		public bool IsCanClaimReward()
		{
			return (data.nextDay - networkTimeManager.GetDateTimeNow().TotalSeconds()) <= 0;
		}

		public bool IsMissedDay()
		{
			return (data.nextDay - networkTimeManager.GetDateTimeNow().TotalSeconds()) < -oneDay.TotalSeconds;
		}

		public bool IsFirstDay()
		{
			return data.nextDayType == DayType.Day1;
		}

		public void SetupNextDay()
		{
			data.lastOpened = networkTimeManager.GetDateTimeNow().TotalSeconds();
			var date = networkTimeManager.GetDateTimeNow();
			date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
			date = date.AddDays(1);//next day
			data.nextDay = date.TotalSeconds();
			data.nextDayType = (DayType)(((int)data.nextDayType + 1) % 8);//next

			analyticsSystem.LogEvent_daily_reward_setup_next_day();

			signalBus?.Fire<SignalSave>();

			onChanged?.Invoke();
		}

		private void Reset()
		{
			SetupNextDay();
			data.nextDayType = DayType.Day1;
			analyticsSystem.LogEvent_daily_reward_reseted();

			signalBus?.Fire<SignalSave>();
		}

		public class Data
		{
			public double lastOpened = 0;
			public double nextDay = 0;
			public DayType nextDayType = DayType.Day1;
		}
	}

	public enum DayType : int
	{
		Day1 = 0,
		Day2 = 1,
		Day3 = 2,
		Day4 = 3,
		Day5 = 4,
		Day6 = 5,
		Day7 = 6,
		Day8 = 7,
	}
}