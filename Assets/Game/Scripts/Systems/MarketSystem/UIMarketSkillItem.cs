using Game.Entities;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public class UIMarketSkillItem : UIMarketItem
	{
		public override void SetPurchasing(IPurchasable purchasable)
		{
			if (CurrentPurchase != null)
			{
				(CurrentPurchase as ActiveSkill).onChanged -= OnPurchasableChanged;
			}

			base.SetPurchasing(purchasable);

			if (CurrentPurchase != null)
			{
				(CurrentPurchase as ActiveSkill).onChanged += OnPurchasableChanged;

				OnPurchasableChanged(CurrentPurchase);
			}
			else
			{
				SetState(BuyType.None);
			}
		}

		public class Factory : PlaceholderFactory<UIMarketSkillItem> { }
	}
}