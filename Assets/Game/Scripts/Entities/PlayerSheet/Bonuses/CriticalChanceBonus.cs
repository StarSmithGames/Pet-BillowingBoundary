using Game.Entities;
using Game.Systems.MarketSystem;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

public class CriticalChanceBonus : Bonus
{
	[SerializeField] private CriticalPowerBonus criticalPowerBonus;

	public override bool IsUnknow { get; protected set; } = false;
	public override int Level { get; protected set; } = 0;
	public override BuyType BuyType { get; protected set; } = BuyType.GET;

	private AddModifier tapModifier;

	private Player player;

	[Inject]
	public void Construct(Player player)
	{
		this.player = player;
	}

	protected override void Start()
	{
		tapModifier = new AddModifier(0f);

		UpdateCost();

		player.TapCriticalChance.AddModifier(tapModifier);

		base.Start();
	}

	public override string GetDescription(bool isReach = true)
	{
		return string.Format(base.GetDescription(), Math.Round(tapModifier.CurrentValue * 100f));
	}

	public override void Purchase()
	{
		if (BuyType == BuyType.GET)
		{
			BuyType = BuyType.UPGADE;
			criticalPowerBonus.SetBuyType(BuyType.UPGADE);
		}

		Level++;
		UpdateEffect();
		UpdateCost();

		base.Purchase();
	}

	protected override void UpdateEffect()
	{
		tapModifier.SetValue(0.01f * Level);//1% by level
	}

	protected override void UpdateCost()
	{
		if (BuyType == BuyType.GET)
		{
			currentCost = new BFN(BonusData.baseCost, 0).compressed;
		}
		else if (BuyType == BuyType.UPGADE)
		{
			currentCost = BFN.FormuleExpoLevelHigh(BonusData.baseCost, Level + 1);
		}

		if (saveLoad.GetStorage().IsBuyFreeMode.GetData())
		{
			currentCost = BFN.Zero;
		}

		base.UpdateCost();
	}
}