using Game.Managers.StorageManager;
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

	public abstract BonusData BonusData { get; }
	public Information Information => BonusData.information;
	public abstract bool IsUnknow { get; protected set; }
	public abstract int Level { get; protected set; }
	public abstract BuyType BuyType { get; protected set; }

	[SerializeField, HideInInspector] private Profile.Data data;

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
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);

		if (!saveLoad.GetStorage().IsFirstTime.GetData())
		{
			data = saveLoad.GetStorage().Profile.GetData();

			Debug.LogError(data.playerData.bonuses.Count);
			for (int i = 0; i < data.playerData.bonuses.Count; i++)
			{
				Debug.LogError(data.playerData.bonuses[i].data.information.name);
			}

			//SetData(data);
		}
	}


	public void SetData(Data data)
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

	public Data GetData()
	{
		return new Data
		{
			data = BonusData,
			isUnknow = IsUnknow,
			level = Level,
			type = BuyType,
		};
	}



	[Button]
	private void Load()
	{
		data = saveLoad.GetStorage().Profile.GetData();
		Debug.LogError(data.playerData.bonuses.Count);
	}

	[Serializable]
	public class Data
	{
		public BonusData data;
		public bool isUnknow;
		public int level;
		public BuyType type;
	}
}