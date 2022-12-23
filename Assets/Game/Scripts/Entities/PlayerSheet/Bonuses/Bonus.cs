using Game.Systems.MarketSystem;
using UnityEngine;
using UnityEngine.Events;

public abstract class Bonus : MonoBehaviour
{
	public event UnityAction onChanged;

	public abstract BonusData BonusData { get; }
	public abstract int Level { get; protected set; }
	public abstract BuyType BuyType { get; protected set; }

	public virtual void LevelUp()
	{
		onChanged?.Invoke();
	}

	public abstract BFN GetCost();
}