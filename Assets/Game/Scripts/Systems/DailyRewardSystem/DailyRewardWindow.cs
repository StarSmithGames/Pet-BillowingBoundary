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
		private NetworkTimeManager networkTimeManager;
		private FloatingAwards floatingAwards;
		private Player player;

		[Inject]
		private void Construct(UISubCanvas subCanvas, ISaveLoad saveLoad,
			DailyRewardSystem dailyRewardSystem,
			NetworkTimeManager networkTimeManager,
			FloatingAwards floatingAwards,
			Player player)
		{
			this.subCanvas = subCanvas;
			this.saveLoad = saveLoad;
			this.dailyRewardSystem = dailyRewardSystem;
			this.networkTimeManager = networkTimeManager;
			this.floatingAwards = floatingAwards;
			this.player = player;
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

			if (saveLoad.GetStorage().IsFirstTime.GetData() == true)//FirstTime show
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

		public override void Show(UnityAction callback = null)
		{
			var data = saveLoad.GetStorage().DailyRewardData.GetData();
			SelectDay(data.currentDay, data.currentState);

			base.Show(callback);
		}

		private void SelectDay(DayType day, DailyRewardState state)
		{
			for (int i = 0; i < rewards.Count; i++)
			{
				if (rewards[i].DayType == day)
				{
					rewards[i].SetState(state, false);
				}
				else if(rewards[i].DayType < day)
				{
					rewards[i].SetState(DailyRewardState.Claimed, false);
				}
				else if (rewards[i].DayType > day)
				{
					rewards[i].SetState(DailyRewardState.Close, false);
				}
			}
		}

		private void OnRewardStateChanged(UIRewardItem rewardItem)
		{
			var data = saveLoad.GetStorage().DailyRewardData.GetData();
			data.currentState = rewardItem.CurrentState;
			if(rewardItem.CurrentState == DailyRewardState.Claimed)
			{
				data.lastOpened = networkTimeManager.GetDateTimeNow().TotalSeconds();

				floatingAwards.StartAwardCoins(rewardItem.transform.position, 25, new BFN(100, 0));
			}

			Assert.IsTrue(data.currentDay == rewardItem.DayType);
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