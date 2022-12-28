using Game.Systems.LocalizationSystem;
using Game.Systems.MarketSystem;

using UnityEngine;
using UnityEngine.Events;

using Zenject;

public abstract class Bonus : MonoBehaviour, IPurchasable
{
	public UnityAction<Bonus> onChanged;

	public abstract BonusData BonusData { get; }
	public Information Information => BonusData.information;
	public abstract bool IsUnknow { get; protected set; }
	public abstract int Level { get; protected set; }
	public abstract BuyType BuyType { get; protected set; }

	protected bool isInitialized = false;
	protected BFN currentCost;

	private LocalizationSystem localizationSystem;

	[Inject]
	private void Construct(LocalizationSystem localizationSystem)
	{
		this.localizationSystem = localizationSystem;
	}

	public virtual string GetName(bool isRich = true)
	{
		if (isRich)
		{
			return BonusData.information.GetName(localizationSystem) + $" <color=#00DF60FF>lvl {Level}</color>";
		}

		return BonusData.information.GetName(localizationSystem) + $" {Level}";
	}

	public virtual string GetDescription(bool isRich = true)
	{
		return BonusData.information.GetDescription(localizationSystem);
	}

	public virtual void Purchase()
	{
		onChanged?.Invoke(this);
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