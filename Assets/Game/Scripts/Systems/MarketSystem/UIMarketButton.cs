using Game.HUD;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public class UIMarketButton : MonoBehaviour
	{
		[field: SerializeField] public Button Button { get; private set; }
		[field: SerializeField] public UIAlert Alert { get; private set; }

		private MarketWindow MarketWindow
		{
			get
			{
				if(marketWindow == null)
				{
					marketWindow = subCanvas.WindowsRegistrator.GetAs<MarketWindow>();
				}
				return marketWindow;
			}
		}
		private MarketWindow marketWindow;

		private UISubCanvas subCanvas;
		private MarketHandler marketHandler;

		[Inject]
		private void Construct(UISubCanvas subCanvas, MarketHandler marketHandler)
		{
			this.subCanvas = subCanvas;
			this.marketHandler = marketHandler;
		}

		private void Start()
		{

			Button.onClick.AddListener(OnClick);
			marketHandler.onValuableChanged += OnValuableChanged;
		}

		private void OnDestroy()
		{
			Button.onClick.RemoveAllListeners();

			if(marketHandler != null)
			{
				marketHandler.onValuableChanged -= OnValuableChanged;
			}
		}

		private void OnValuableChanged()
		{
			if (MarketWindow.IsShowing) return;

			if (marketHandler.IsCanBoughtSomething)
			{
				if (!Alert.IsShowing && !Alert.IsInProcess)
				{

					Alert.Show();
				}
			}
		}

		private void OnClick()
		{
			MarketWindow.Show();

			if (Alert.IsShowing)
			{
				Alert.Hide();
			}
		}
	}
}