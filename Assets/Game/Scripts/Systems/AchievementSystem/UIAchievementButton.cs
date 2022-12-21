using Game.UI;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.AchievementSystem
{
	public class UIAchievementButton : MonoBehaviour
	{
		[field: SerializeField] public Button Button { get; private set; }

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
			subCanvas.WindowsRegistrator.Show<AchievementWindow>();
		}
	}
}