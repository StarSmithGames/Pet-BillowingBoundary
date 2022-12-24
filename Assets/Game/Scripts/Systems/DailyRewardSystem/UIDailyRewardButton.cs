using Game.HUD;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.DailyRewardSystem
{
    public class UIDailyRewardButton : MonoBehaviour
    {
		[field: SerializeField] public Button Button { get; private set; }
		[field: SerializeField] public UIAlert Alert { get; private set; }

		private UISubCanvas subCanvas;

		[Inject]
		private void Construct(UISubCanvas subCanvas)
		{
			this.subCanvas = subCanvas;
		}

		private void Start()
		{
			Button.onClick.AddListener(OnClick);
		}

		private void OnDestroy()
		{
			Button.onClick.RemoveAllListeners();
		}

		private void OnClick()
		{
			subCanvas.WindowsRegistrator.Show<DailyRewardWindow>();
		}
	}
}