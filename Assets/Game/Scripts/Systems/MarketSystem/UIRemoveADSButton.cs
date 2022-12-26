using Game.Managers.IAPManager;
using Game.UI;

using UnityEngine;
using UnityEngine.Monetization;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public class UIRemoveADSButton : UIButton
	{
		private IAPManager iapManager;

		[Inject]
		private void Construct(IAPManager iapManager)
		{
			this.iapManager = iapManager;
		}

		protected override void OnClick()
		{
			iapManager.BuyProductID(iapManager.removeADS);
		}
	}
}