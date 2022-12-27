using Game.HUD;
using Game.UI;
using UnityEngine;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public class UIMarketButton : UIButton
	{
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

		protected override void Start()
		{
			base.Start();
			marketHandler.onValuableChanged += OnValuableChanged;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
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

		protected override void OnClick()
		{
			MarketWindow.Show();

			if (Alert.IsShowing)
			{
				Alert.Hide();
			}

			base.OnClick();
		}
	}
}