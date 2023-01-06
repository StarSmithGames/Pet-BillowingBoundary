using Game.Managers.StorageManager;
using Game.Systems.AnalyticsSystem;
using Game.Systems.LocalizationSystem;
using Game.Systems.MarketSystem;

using Sirenix.OdinInspector;

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

using Zenject;

public abstract class Bonus : MonoBehaviour, IPurchasable
{
	public UnityAction<Bonus> onChanged;

	public BonusData BonusData => bonusData;
	[SerializeField] private BonusData bonusData;

	public Information Information => BonusData.information;
	public abstract bool IsUnknow { get; protected set; }
	public abstract int Level { get; protected set; }
	public abstract BuyType BuyType { get; protected set; }

	protected bool isInitialized = false;
	protected BFN currentCost;

	protected SignalBus signalBus;
	private ISaveLoad saveLoad;
	private LocalizationSystem localizationSystem;
	private AnalyticsSystem analyticsSystem;

	[Inject]
	private void Construct(
		SignalBus signalBus,
		ISaveLoad saveLoad,
		LocalizationSystem localizationSystem,
		AnalyticsSystem analyticsSystem)
	{
		this.signalBus = signalBus;
		this.saveLoad = saveLoad;
		this.localizationSystem = localizationSystem;
		this.analyticsSystem = analyticsSystem;
	}

	protected virtual void Start()
	{
		if (!saveLoad.GetStorage().IsFirstTime.GetData())
		{
			SetData(saveLoad.GetStorage().Profile.GetData().playerData.bonuses.Find((x) => x.data == BonusData));
		}
	}

	public void SetData(BonusSaveData data)
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

		signalBus?.Fire<SignalSave>();

		analyticsSystem.LogEvent_bonus_upgraded(BonusData.id);
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

	public BonusSaveData GetData()
	{
		return new BonusSaveData
		{
			data = BonusData,
			isUnknow = IsUnknow,
			level = Level,
			type = BuyType,
		};
	}
}

[Serializable]
public class BonusSaveData
{
	public BonusData data;
	public bool isUnknow;
	public int level;
	public BuyType type;
}