using Game.Entities;
using Game.Systems.MarketSystem;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknowSkill : ActiveSkill
{
	public override SkillData SkillData => data;
	[SerializeField] private FireFistSkillData data;
	public override bool IsUnknow { get; protected set; } = true;
	public override BuyType BuyType { get; protected set; } = BuyType.None;

	public override SkillProperty GetProperty(int index)
	{
		return null;
	}

	public override void PurchaseProperty(int index)
	{
	}
}