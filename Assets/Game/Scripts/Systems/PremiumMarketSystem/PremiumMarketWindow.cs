using Game.UI;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.PremiumMarketSystem
{
	public class PremiumMarketWindow : WindowPopupBase
	{
		[field: SerializeField] public Button Blank { get; private set; }
		[field: SerializeField] public Button Close { get; private set; }

		private UISubCanvas subCanvas;

		[Inject]
		private void Construct(UISubCanvas subCanvas)
		{
			this.subCanvas = subCanvas;
		}

		private void Start()
		{
			Enable(false);

			Blank.onClick.AddListener(OnClosed);
			Close.onClick.AddListener(OnClosed);

			subCanvas.WindowsRegistrator.Registrate(this);
		}

		private void OnDestroy()
		{
			Blank?.onClick.RemoveAllListeners();
			Close?.onClick.RemoveAllListeners();

			subCanvas.WindowsRegistrator.UnRegistrate(this);
		}

		private void OnClosed()
		{
			Hide();
		}
	}
}