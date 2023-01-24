using DG.Tweening;

using Game.Entities;
using Game.HUD;
using Game.Managers.NetworkTimeManager;
using Game.Managers.StorageManager;
using Game.Systems.FloatingSystem;
using Game.UI;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.DailyRewardSystem
{
	public class DailyRewardWindow : WindowQuartBase
	{
		[field: SerializeField] public Button Blank { get; private set; }
		[field: SerializeField] public Button Close { get; private set; }
		
		[SerializeField] private List<UIRewardItem> rewards = new List<UIRewardItem>();

		private UISubCanvas subCanvas;
		private ISaveLoad saveLoad;
		private DailyRewardSystem dailyRewardSystem;
		private FloatingAwards floatingAwards;
		private Player player;
		private AnalyticsSystem.AnalyticsSystem analyticsSystem;

		[Inject]
		private void Construct(UISubCanvas subCanvas,
			ISaveLoad saveLoad,
			DailyRewardSystem dailyRewardSystem,
			FloatingAwards floatingAwards,
			Player player,
			AnalyticsSystem.AnalyticsSystem analyticsSystem)
		{
			this.subCanvas = subCanvas;
			this.saveLoad = saveLoad;
			this.dailyRewardSystem = dailyRewardSystem;
			this.floatingAwards = floatingAwards;
			this.player = player;
			this.analyticsSystem = analyticsSystem;
		}

		private void Start()
		{
			Enable(false);

			Blank.onClick.AddListener(OnClosed);
			Close.onClick.AddListener(OnClosed);

			for (int i = 0; i < rewards.Count; i++)
			{
				rewards[i].onRewardStateChanged += OnRewardStateChanged;
			}

			subCanvas.WindowsRegistrator.Registrate(this);

			if (saveLoad.GetStorage().IsFirstTime.GetData() == false && dailyRewardSystem.IsHasReward())
			{
				Show();
			}
		}

		private void OnDestroy()
		{
			Blank?.onClick.RemoveAllListeners();
			Close?.onClick.RemoveAllListeners();

			for (int i = 0; i < rewards.Count; i++)
			{
				rewards[i].onRewardStateChanged -= OnRewardStateChanged;
			}

			subCanvas.WindowsRegistrator.UnRegistrate(this);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Debug.LogError(saveLoad.GetStorage().DailyRewardData.GetData().nextDayType);
				var reward = rewards.Find((x) => x.DayType == saveLoad.GetStorage().DailyRewardData.GetData().nextDayType);
				reward.SetState(DailyRewardState.Claimed);

				UpdateUI();
			}
		}

		public override void Show(UnityAction callback = null)
		{
			UpdateUI();

			base.Show(callback);
		}

		private void UpdateUI()
		{
			var dailyRewards = dailyRewardSystem.GetRewards();

			Assert.IsTrue(dailyRewards.Count == rewards.Count);

			var data = saveLoad.GetStorage().DailyRewardData.GetData();
			for (int i = 0; i < rewards.Count; i++)
			{
				int diff = (int)rewards[i].DayType - (int)data.nextDayType;

				if (diff == 0)
				{
					rewards[i].SetState(dailyRewardSystem.IsHasReward() ? DailyRewardState.Open : DailyRewardState.Close, false);
				}
				else if (diff > 0)
				{
					rewards[i].SetState(DailyRewardState.Close, false);
				}
				else
				{
					rewards[i].SetState(DailyRewardState.Claimed, false);
				}
			}

			for (int i = 0; i < dailyRewards.Count; i++)
			{
				rewards[i].SetReward(dailyRewards[i]);
			}
		}

		private void OnRewardStateChanged(UIRewardItem rewardItem)
		{
			var data = saveLoad.GetStorage().DailyRewardData.GetData();

			Assert.IsTrue(data.nextDayType == rewardItem.DayType);

			if (rewardItem.CurrentState == DailyRewardState.Claimed)
			{
				floatingAwards.StartAwardCoins(rewardItem.transform.position, rewardItem.TotalCoins);

				dailyRewardSystem.SetupNextDay();

				analyticsSystem.LogEvent_daily_reward_claimed();
			}
		}

		private void OnClosed()
		{
			Hide();
		}

		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			rewards = GetComponentsInChildren<UIRewardItem>().ToList();
		}
	}
}