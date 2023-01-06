using Game.Entities;
using Game.Managers.StorageManager;
using Game.Systems.MarketSystem;

using Sirenix.OdinInspector;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization.Settings;

using Zenject;

public class TapDamageBonus : Bonus
{
	public override bool IsUnknow { get; protected set; } = false;
	public override int Level { get; protected set; } = 0;
	public override BuyType BuyType { get; protected set; } = BuyType.UPGADE;

	public float CurrentDamage => Level;

	private AddModifierBFN tapModifier;

	private Player player;

	[Inject]
	public void Construct(Player player)
	{
		this.player = player;
	}

	protected override void Start()
	{
		tapModifier = new AddModifierBFN(new BFN(CurrentDamage, 0).compressed);

		UpdateCost();

		player.TapDamage.AddModifier(tapModifier);

		base.Start();
	}

	public override void Purchase()
	{
		Level++;
		
		UpdateEffect();
		UpdateCost();

		base.Purchase();
	}

	protected override void UpdateEffect()
	{
		tapModifier.SetValue(new BFN(CurrentDamage, 0).compressed);//1 tap by level
	}

	protected override void UpdateCost()
	{
		if(Level == 0)
		{
			currentCost = new BFN(BonusData.baseCost, 0).compressed;
		}
		else
		{
			currentCost = BFN.FormuleExpoLevelLow(BonusData.baseCost, Level + 1);
		}

		if (saveLoad.GetStorage().IsBuyFreeMode.GetData())
		{
			currentCost = BFN.Zero;
		}

		base.UpdateCost();
	}
}