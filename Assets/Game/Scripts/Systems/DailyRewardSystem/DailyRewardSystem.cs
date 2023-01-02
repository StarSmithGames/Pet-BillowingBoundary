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

		public void RefreshDays()
		{
			SetupNextDay();

			onChanged?.Invoke();
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

		private void SetupNextDay()
		{
			var date = networkTimeManager.GetDateTimeNow();
			date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
			date = date.AddDays(1);
			data.nextDay = date.TotalSeconds();
			data.currentDay = (DayType)(((int)data.currentDay + 1) % 9);//next
		}

		private void Reset()
		{
			SetupNextDay();
			data.lastOpened = networkTimeManager.GetDateTimeNow().TotalSeconds();
			data.currentDay = DayType.Day1;
			data.currentState = DailyRewardState.Open;

			analyticsSystem.LogEvent_daily_reward_reseted();
		}

		public class Data
		{
			public double nextDay = 0;
			public double lastOpened = -1;
			public DayType currentDay = DayType.Day1;
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