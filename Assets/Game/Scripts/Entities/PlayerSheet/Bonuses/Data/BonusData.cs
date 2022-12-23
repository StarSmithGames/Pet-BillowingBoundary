using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BonusData : ScriptableObject
{
	[HideLabel]
	public Information information;

	public bool isIconSimple = true;

	[Space]
	public bool isUnlockedOnStart = false;
	[HideIf("isUnlockedOnStart")]
	public int unlockCost;
	public int baseCost;
	[Space]
	public bool isHasMaxLevel = true;
	[ShowIf("isHasMaxLevel")]
	[Min(1)]
	public int maxLevel = 100;
}