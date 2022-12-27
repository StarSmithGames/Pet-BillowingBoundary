using Game.HUD;
using Game.UI;
using UnityEngine;

using Zenject;

namespace Game.Systems.DailyRewardSystem
{
    public class UIDailyRewardButton : UIButton
    {
		[field: SerializeField] public UIAlert Alert { get; private set; }

		private UISubCanvas subCanvas;

		[Inject]
		private void Construct(UISubCanvas subCanvas)
		{
			this.subCanvas = subCanvas;
		}

		protected override void OnClick()
		{
			subCanvas.WindowsRegistrator.Show<DailyRewardWindow>();

			base.OnClick();
		}
	}
}