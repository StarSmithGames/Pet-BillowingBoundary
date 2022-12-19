using Game.UI;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.LocalizationSystem
{
    public class LanguageWindow : WindowBase
    {
		[field: SerializeField] public Button Open { get; private set; }
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

			Open.onClick.AddListener(OnOpened);
			Close.onClick.AddListener(OnClosed);

			subCanvas.WindowsRegistrator.Registrate(this);
		}

		private void OnDestroy()
		{
			Open?.onClick.RemoveAllListeners();
			Close?.onClick.RemoveAllListeners();

			subCanvas.WindowsRegistrator.UnRegistrate(this);
		}

		private void OnOpened()
		{
			Show();
		}

		private void OnClosed()
		{
			Hide();
		}
	}
}