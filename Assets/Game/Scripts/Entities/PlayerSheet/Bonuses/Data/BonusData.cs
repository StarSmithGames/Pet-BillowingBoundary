using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusData", menuName = "Game/Bonus")]
public class BonusData : ScriptableObject
{
	[HideLabel]
	public Information information;

	public bool isIconSimple = true;

	[Space]
	public int baseCost;
	[Space]
	public bool isHasMaxLevel = true;
	[ShowIf("isHasMaxLevel")]
	[Min(1)]
	public int maxLevel = 100;
}