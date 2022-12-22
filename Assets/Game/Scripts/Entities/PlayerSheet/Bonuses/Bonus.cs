using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class Bonus : MonoBehaviour
{
	public abstract BonusData BonusData { get; }

	public virtual void LevelUp()
	{

	}

	public virtual float GetCost()
	{
		return 0;
	}
}