using Game.UI;
using Zenject;

namespace Game.Systems.MarketSystem
{
	public class UIMarketBonusItem : UIMarketItem
	{
		public override void SetPurchasing(IPurchasable purchasable)
		{
			if (CurrentPurchase != null)
			{
				(CurrentPurchase as Bonus).onChanged -= OnPurchasableChanged;
			}

			base.SetPurchasing(purchasable);

			if (CurrentPurchase != null)
			{
				(CurrentPurchase as Bonus).onChanged += OnPurchasableChanged;

				OnPurchasableChanged(CurrentPurchase);
				OnLocalizationChanged();
			}
			else
			{
				SetState(BuyType.None);
			}
		}

		protected override void OnLockClick()
		{
			var window = fastMessagesFactory.Create();
			window.Show(FastMessageType.LockedBonus);
		}

		public class Factory : PlaceholderFactory<UIMarketBonusItem> { }
	}
}