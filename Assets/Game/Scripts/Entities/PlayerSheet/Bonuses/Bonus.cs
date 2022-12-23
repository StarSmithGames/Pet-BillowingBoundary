using Game.Systems.MarketSystem;
using System;

using UnityEngine;
using UnityEngine.Events;

public abstract class Bonus : MonoBehaviour
{
	public event UnityAction onChanged;

	public abstract BonusData BonusData { get; }
	public abstract int Level { get; protected set; }
	public abstract BuyType BuyType { get; protected set; }

	protected bool isInitialized = false;
	protected BFN currentCost;

	public virtual void Purchase()
	{
		onChanged?.Invoke();
	}

	public BFN GetCost()
	{
		if (!isInitialized)
		{
			UpdateCost();
		}

		return currentCost;
	}

	protected virtual void UpdateCost()
	{
		isInitialized = true;
	}
}