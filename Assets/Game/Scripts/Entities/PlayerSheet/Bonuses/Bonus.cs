using Game.Managers.StorageManager;
using Game.Systems.LocalizationSystem;
using Game.Systems.MarketSystem;

using System;

using UnityEngine;
using UnityEngine.Assertions;
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

	private ISaveLoad saveLoad;
	private LocalizationSystem localizationSystem;

	[Inject]
	private void Construct(ISaveLoad saveLoad, LocalizationSystem localizationSystem)
	{
		this.saveLoad = saveLoad;
		this.localizationSystem = localizationSystem;
	}
	private void Start()
	{
		if (!saveLoad.GetStorage().IsFirstTime.GetData())
		{
			var data = saveLoad.GetStorage().Profile.GetData().playerData.bonuses;

			Debug.LogError(data.bonuses.Count);
			for (int i = 0; i < data.bonuses.Count; i++)
			{
				Debug.LogError(data.bonuses[i].bonus.information.name);
			}

			Assert.IsTrue(data != null);

			//SetData(data);
		}
	}


	public void SetData(BonusDataSave data)
	{
		IsUnknow = data.isUnknow;
		Level = data.level;
		BuyType = data.type;

		UpdateCost();
		UpdateEffect();

		onChanged?.Invoke(this);
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

	protected virtual void UpdateEffect()
	{

	}

	protected virtual void UpdateCost()
	{
		isInitialized = true;
	}

	public BonusDataSave GetData()
	{
		return new BonusDataSave
		{
			bonus = BonusData,
			isUnknow = IsUnknow,
			level = Level,
			type = BuyType,
		};
	}
}
[Serializable]
public class BonusDataSave
{
	public BonusData bonus;
	public bool isUnknow;
	public int level;
	public BuyType type;
}