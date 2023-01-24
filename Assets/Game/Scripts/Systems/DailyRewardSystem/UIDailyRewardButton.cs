using Game.HUD;
using Game.Managers.StorageManager;
using Game.UI;

using System.Collections;
using System.Threading;

using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.DailyRewardSystem
{
    public class UIDailyRewardButton : UIAnimatedButton
	{
		[field: SerializeField] public UIAlert Alert { get; private set; }

		private UISubCanvas subCanvas;

		private ISaveLoad saveLoad;
		private DailyRewardSystem dailyRewardSystem;

		[Inject]
		private void Construct(UISubCanvas subCanvas, ISaveLoad saveLoad, DailyRewardSystem dailyRewardSystem)
		{
			this.subCanvas = subCanvas;
			this.saveLoad = saveLoad;
			this.dailyRewardSystem = dailyRewardSystem;
		}

		protected override void Start()
		{
			base.Start();

			Enable(false);

			StartCoroutine(ShowWithDelay(!saveLoad.GetStorage().IsFirstTime.GetData() ? 2f : 15f));

			dailyRewardSystem.onChanged += CheckOnAlert;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			dailyRewardSystem.onChanged -= CheckOnAlert;
		}

		private IEnumerator ShowWithDelay(float delay)
		{
			yield return new WaitForSeconds(delay);

			Show(() =>
			{
				if (dailyRewardSystem.IsHasReward())
				{
					Alert.Show();
				}
			});
		}

		private void CheckOnAlert()
		{
			if (dailyRewardSystem.IsHasReward())
			{
				if (!Alert.IsShowing)
				{
					Alert.Show();
				}
			}
			else
			{
				if (Alert.IsShowing)
				{
					Alert.Hide();
				}
			}
		}

		protected override void OnClick()
		{
			subCanvas.WindowsRegistrator.Show<DailyRewardWindow>();

			base.OnClick();
		}
	}
}