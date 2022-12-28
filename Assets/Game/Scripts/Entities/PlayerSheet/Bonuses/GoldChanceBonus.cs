using Game.Entities;
using Game.Systems.MarketSystem;
using System;
using UnityEngine;

using Zenject;

public class GoldChanceBonus : Bonus
{
	public override BonusData BonusData => data;
	[SerializeField] private BonusData data;

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

		player.TapGoldChance.AddModifier(tapModifier);
	}

	public override string GetDescription(bool isReach = true)
	{
		return string.Format(base.GetDescription(), Math.Round(tapModifier.CurrentValue * 100f));
	}

	public override void Purchase()
	{
		if(BuyType == BuyType.GET)
		{
			BuyType = BuyType.UPGADE;
		}

		Level++;
		tapModifier.SetValue(0.01f * Level);//1% by level

		UpdateCost();

		base.Purchase();
	}

	protected override void UpdateCost()
	{
		if(BuyType == BuyType.GET)
		{
			currentCost = new BFN(data.baseCost, 0).compressed;
		}
		else if(BuyType == BuyType.UPGADE)
		{
			currentCost = new BFN(Math.Ceiling(data.baseCost * (Mathf.Pow(1.07f, Level + 1))), 0).compressed;
		}

		base.UpdateCost();
	}
}