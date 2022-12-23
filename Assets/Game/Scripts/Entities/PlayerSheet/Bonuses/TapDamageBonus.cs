using Game.Entities;
using Game.Systems.MarketSystem;

using System;

using UnityEngine;

using Zenject;

public class TapDamageBonus : Bonus
{
	public override BonusData BonusData => data;
	[SerializeField] private BonusData data;
	public override int Level { get; protected set; } = 0;
	public override BuyType BuyType { get; protected set; } = BuyType.BUY;

	public float CurrentDamage => Level;

	private AddModifier tapModifier;

	private Player player;

	[Inject]
	public void Construct(Player player)
	{
		this.player = player;
	}

	private void Start()
	{
		tapModifier = new AddModifier(1);
		player.BonusRegistrator.Registrate(this);

		player.TapDamage.AddModifier(tapModifier);
	}

	private void OnDestroy()
	{
		player?.BonusRegistrator.UnRegistrate(this);
	}

	public override void LevelUp()
	{
		Level++;
		tapModifier.SetValue(Level);

		base.LevelUp();
	}

	public override float GetCost()
	{
		return (float)Math.Ceiling(data.baseCost * (Mathf.Pow(1.15f, Level)));
	}

	public class Data
	{
		public int level;
	}
}

enum Multiplier
{
	Thousand,//K = 1000
	Million,//M = 1000K
	Billion,//B = 1000M
	Trillion,//T = 1000B
	Quadrillion,//q = 1000T
	Quatrillion,//Q = 1000q
	Sextillion,//s = 1000Q
}