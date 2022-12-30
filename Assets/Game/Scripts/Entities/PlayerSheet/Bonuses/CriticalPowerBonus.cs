using Game.Entities;
using Game.Systems.MarketSystem;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

public class CriticalPowerBonus : Bonus
{
	public override BonusData BonusData => data;
	[SerializeField] private BonusData data;
	public override bool IsUnknow { get; protected set; } = false;
	public override int Level { get; protected set; } = 0;
	public override BuyType BuyType { get; protected set; } = BuyType.LOCK;

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

		player.TapCriticalPower.AddModifier(tapModifier);
	}

	public void SetBuyType(BuyType buyType)
	{
		BuyType = buyType;

		tapModifier.SetValue(0.01f);//1% by level

		onChanged?.Invoke(this);
	}

	public override string GetDescription(bool isReach = true)
	{
		return string.Format(base.GetDescription(), Math.Round(player.TapCriticalPower.TotalValue, 2));
	}

	public override void Purchase()
	{
		if (BuyType == BuyType.LOCK)
		{
			return;
		}

		Level++;
		UpdateEffect();
		UpdateCost();

		base.Purchase();
	}

	protected override void UpdateEffect()
	{
		tapModifier.SetValue(0.01f * (Level + 1));//1% by level
	}

	protected override void UpdateCost()
	{
		if(Level == 0)
		{
			currentCost = new BFN(data.baseCost, 0);
		}
		else
		{
			currentCost = BFN.FormuleExpoLevelHigh(data.baseCost, Level + 1);
		}

		base.UpdateCost();
	}
}