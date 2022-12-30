﻿using Game.Entities;
using Game.Systems.MarketSystem;

using System;

using UnityEngine;
using UnityEngine.Localization.Settings;

using Zenject;

public class TapDamageBonus : Bonus
{
	public override BonusData BonusData => data;
	[SerializeField] private BonusData data;
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

	private void Start()
	{
		tapModifier = new AddModifierBFN(new BFN(CurrentDamage, 0).compressed);

		UpdateCost();

		player.TapDamage.AddModifier(tapModifier);
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
			currentCost = new BFN(data.baseCost, 0).compressed;
		}
		else
		{
			currentCost = BFN.FormuleExpoLevelLow(data.baseCost, Level + 1);
		}
		base.UpdateCost();
	}
}