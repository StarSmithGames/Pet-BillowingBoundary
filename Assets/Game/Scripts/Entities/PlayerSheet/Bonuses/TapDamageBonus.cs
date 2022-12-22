using Game.Entities;

using UnityEngine;

public class TapDamageBonus : Bonus
{
	public override BonusData BonusData => data;
	[SerializeField] private BonusData data;

	public float CurrentDamage => currentLevel;

	private int currentLevel;

	private Player player;

	public void Construct(Player player)
	{
		this.player = player;
	}

	private void Start()
	{
		player.BonusRegistrator.Registrate(this);
	}

	private void OnDestroy()
	{
		player?.BonusRegistrator.UnRegistrate(this);
	}

	public void LevelUp()
	{
		currentLevel++;
	}

	public float GetCost()
	{
		return data.baseCost * (Mathf.Pow(1.07f, currentLevel));
	}

	public class Data
	{
		public int level;
	}
}