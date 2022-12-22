using Game.Entities;

using UnityEngine;

using Zenject;

public class TapDamageBonus : Bonus
{
	public override BonusData BonusData => data;
	[SerializeField] private BonusData data;

	public float CurrentDamage => currentLevel;

	private int currentLevel;

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
		currentLevel++;
		tapModifier.SetValue(currentLevel);
	}

	public override float GetCost()
	{
		return data.baseCost * (Mathf.Pow(1.07f, currentLevel));
	}

	public class Data
	{
		public int level;
	}
}