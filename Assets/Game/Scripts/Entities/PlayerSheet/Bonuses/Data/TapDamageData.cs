using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TapDamageData", menuName = "Game/Bonuses/TapDamage")]
public class TapDamageData : BonusData
{
	[Min(1)]
	public float initialDamage = 1;
}