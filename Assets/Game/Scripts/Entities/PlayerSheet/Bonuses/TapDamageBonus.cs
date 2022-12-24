using Game.Entities;
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
		tapModifier.SetValue(new BFN(CurrentDamage, 0).compressed);//1 tap by level

		UpdateCost();

		base.Purchase();
	}

	protected override void UpdateCost()
	{
		currentCost = new BFN(Math.Ceiling(data.baseCost * (Mathf.Pow(1.07f, Level + 1))), 0).compressed;
		base.UpdateCost();
	}

	public class Data
	{
		public int level;
	}
}