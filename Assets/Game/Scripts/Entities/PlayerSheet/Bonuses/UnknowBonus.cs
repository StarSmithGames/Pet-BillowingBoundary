using Game.Systems.MarketSystem;
using UnityEngine;

public class UnknowBonus : Bonus
{
	public override bool IsUnknow { get; protected set; } = true;
	public override int Level { get; protected set; } = 0;
	public override BuyType BuyType { get; protected set; } = BuyType.None;
}