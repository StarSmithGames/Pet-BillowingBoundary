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

	public override int Level { get; protected set; } = 0;
	public override BuyType BuyType { get; protected set; } = BuyType.UPGADE;

	public float CurrentDamage => Level;

	private AddModifierBFN tapModifier;
	private bool isInitialized = false;
	private BFN currentCost;

	private Player player;

	[Inject]
	public void Construct(Player player)
	{
		this.player = player;
	}

	private void Start()
	{
		tapModifier = new AddModifierBFN(new BFN(CurrentDamage, 0).compressed);
		player.BonusRegistrator.Registrate(this);

		UpdateCost();

		player.TapDamage.AddModifier(tapModifier);
	}

	private void OnDestroy()
	{
		player?.BonusRegistrator.UnRegistrate(this);
	}

	public override void LevelUp()
	{
		Level++;
		tapModifier.SetValue(new BFN(CurrentDamage, 0).compressed);

		UpdateCost();

		base.LevelUp();
	}

	public override BFN GetCost()
	{
		if (!isInitialized)
		{
			UpdateCost();
		}

		return currentCost;
	}
	private void UpdateCost()
	{
		currentCost = new BFN(Math.Ceiling(data.baseCost * (Mathf.Pow(1.15f, Level + 1))), 0).compressed;
		isInitialized = true;
	}

	public class Data
	{
		public int level;
	}
}