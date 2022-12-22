using Game.Systems.LocalizationSystem;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public class UIMarketItem : PoolableObject
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Title { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Description { get; private set; }
		[field: Space]
		[field: SerializeField] public UIBuyButton Buy { get; private set; }
		[field: SerializeField] public UIBuyButton Get { get; private set; }
		[field: SerializeField] public UIBuyButton Upgrade { get; private set; }
		[field: Space]
		[field: SerializeField] public UIIcon IconSimple { get; private set; }
		[field: SerializeField] public UIIcon IconFull { get; private set; }
		[field: Space]
		[field: SerializeField] public GameObject Separator { get; private set; }

		private BuyType type;
		private Bonus bonus;
		private Information information;

		private SignalBus signalBus;
		private LocalizationSystem.LocalizationSystem localizationSystem;

		[Inject]
		private void Construct(SignalBus signalBus, LocalizationSystem.LocalizationSystem localizationSystem)
		{
			this.signalBus = signalBus;
			this.localizationSystem = localizationSystem;
		}

		private void Start()
		{
			signalBus?.Subscribe<SignalLocalizationChanged>(OnLocalizationChanged);
		}

		private void OnDestroy()
		{
			signalBus?.Unsubscribe<SignalLocalizationChanged>(OnLocalizationChanged);
		}

		public void SetState(BuyType type)
		{
			this.type = type;
		}

		public void SetBonus(Bonus bonus)
		{
			this.bonus = bonus;
			this.information = bonus.BonusData.information;

			if (bonus.BonusData.isIconSimple)
			{
				IconSimple.Icon.sprite = information.portrait;
				IconSimple.Shadow.sprite = information.portrait;
			}
			else
			{
				IconFull.Icon.sprite = information.portrait;
			}
			IconSimple.gameObject.SetActive(bonus.BonusData.isIconSimple);
			IconFull.gameObject.SetActive(!bonus.BonusData.isIconSimple);

			Buy.SetText(bonus.GetCost().ToString());
			Get.SetText(bonus.GetCost().ToString());
			Upgrade.SetText(bonus.GetCost().ToString());

			OnLocalizationChanged();
		}

		public void EnbleSeparator(bool trigger)
		{
			Separator.SetActive(trigger);
		}

		private void OnLocalizationChanged()
		{
			if (bonus == null) return;

			Title.text = information.isNameId ? localizationSystem.Translate(information.name) : information.name;
			Description.text = information.isDescriptionId ? localizationSystem.Translate(information.description) : information.description;
		}

		public class Factory : PlaceholderFactory<UIMarketItem> { }
	}

	public enum BuyType
	{
		None,

		BUY,
		GET,
		UPGADE,
		LOCK,
	}
}