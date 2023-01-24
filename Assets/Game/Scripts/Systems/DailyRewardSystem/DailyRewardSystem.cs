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

		private DailyRewardSetting setting;
		private UISubCanvas subCanvas;
		private ISaveLoad saveLoad;
		private NetworkTimeManager networkTimeManager;
		private AnalyticsSystem.AnalyticsSystem analyticsSystem;

		public DailyRewardSystem(
			DailyRewardSetting setting,
			UISubCanvas subCanvas,
			ISaveLoad saveLoad,
			NetworkTimeManager networkTimeManager,
			AnalyticsSystem.AnalyticsSystem analyticsSystem)
        {
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
			return saveLoad.GetStorage().DailyRewardData.GetData().currentState == DailyRewardState.Open;
		}

		public bool IsMissedDay()
		{
			return GetLastTime() < -oneDay.TotalSeconds;
		}

		public double GetLastTime()
		{
			return data.nextDay - networkTimeManager.GetDateTimeNow().TotalSeconds();
		}

		public void SetupNextDay()
		{
			data.lastOpened = networkTimeManager.GetDateTimeNow().TotalSeconds();
			var date = networkTimeManager.GetDateTimeNow();
			date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
			date = date.AddDays(1);//next day
			data.nextDay = date.TotalSeconds();
			data.nextDayType = (DayType)(((int)data.nextDayType + 1) % 9);//next

			analyticsSystem.LogEvent_daily_reward_setup_next_day();

			onChanged?.Invoke();
		}

		private void Reset()
		{
			SetupNextDay();
			data.nextDayType = DayType.Day1;
			data.currentState = DailyRewardState.Open;

			analyticsSystem.LogEvent_daily_reward_reseted();
		}

		public class Data
		{
			public double lastOpened = -1;

			public double nextDay = 0;
			public DayType nextDayType = DayType.Day1;
			public DailyRewardState currentState = DailyRewardState.Open;
		}
	}

	public enum DayType : int
	{
		Day1 = 1,
		Day2 = 2,
		Day3 = 3,
		Day4 = 4,
		Day5 = 5,
		Day6 = 6,
		Day7 = 7,
		Day8 = 8,
	}
}