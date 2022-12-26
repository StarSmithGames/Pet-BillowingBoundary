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
	public override BonusData BonusData => data;
	[SerializeField] private BonusData data;
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

	private void Start()
	{
		tapModifier = new AddModifier(0f);


		UpdateCost();

		player.TapCriticalChance.AddModifier(tapModifier);
	}

	public override string GetDescription()
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
		tapModifier.SetValue(0.01f * Level);//1% by level

		UpdateCost();

		base.Purchase();
	}

	protected override void UpdateCost()
	{
		if (BuyType == BuyType.GET)
		{
			currentCost = new BFN(data.baseCost, 0).compressed;
		}
		else if (BuyType == BuyType.UPGADE)
		{
			currentCost = new BFN(Math.Ceiling(data.baseCost * (Mathf.Pow(1.15f, Level + 1))), 0).compressed;
		}

		base.UpdateCost();
	}
}