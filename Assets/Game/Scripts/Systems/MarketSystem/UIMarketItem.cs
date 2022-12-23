using Game.Entities;
using Game.Systems.LocalizationSystem;
using System;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public class UIMarketItem : PoolableObject
	{
		public UnityAction<UIMarketItem> onBuyClick;

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

		public Bonus CurrentBonus { get; private set; }

		private Information information;
		private UIBuyButton currentButton;

		private SignalBus signalBus;
		private Player player;
		private LocalizationSystem.LocalizationSystem localizationSystem;

		[Inject]
		private void Construct(SignalBus signalBus, Player player, LocalizationSystem.LocalizationSystem localizationSystem)
		{
			this.signalBus = signalBus;
			this.player = player;
			this.localizationSystem = localizationSystem;
		}

		private void Start()
		{
			Buy.Button.onClick.AddListener(OnBuyClick);
			Get.Button.onClick.AddListener(OnBuyClick);
			Upgrade.Button.onClick.AddListener(OnBuyClick);

			player.Gold.onChanged += GoldCheck;

			signalBus?.Subscribe<SignalLocalizationChanged>(OnLocalizationChanged);
		}

		private void OnDestroy()
		{
			Buy?.Button.onClick.RemoveAllListeners();
			Get?.Button.onClick.RemoveAllListeners();
			Upgrade?.Button.onClick.RemoveAllListeners();

			if(player != null)
			{
				player.Gold.onChanged -= GoldCheck;
			}

			signalBus?.Unsubscribe<SignalLocalizationChanged>(OnLocalizationChanged);
		}

		public void SetBonus(Bonus bonus)
		{
			if(CurrentBonus != null)
			{
				CurrentBonus.onChanged -= OnBonusChanged;
			}

			CurrentBonus = bonus;

			if (CurrentBonus != null)
			{
				CurrentBonus.onChanged += OnBonusChanged;

				information = bonus.BonusData.information;

				//Icon
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

				SetState(bonus.BuyType);
				OnBonusChanged();
				OnLocalizationChanged();
			}
			else
			{
				SetState(BuyType.None);
			}
		}

		private void SetState(BuyType type)
		{
			currentButton = null;

			Buy.gameObject.SetActive(false);
			Get.gameObject.SetActive(false);
			Upgrade.gameObject.SetActive(false);

			switch (type)
			{
				case BuyType.BUY:
				{
					currentButton = Buy;
					break;
				}
				case BuyType.GET:
				{
					currentButton = Get;
					break;
				}
				case BuyType.UPGADE:
				{
					currentButton = Upgrade;
					break;
				}
				case BuyType.LOCK:
				{
					break;
				}

				default:
				{

					break;
				}
			}

			currentButton?.gameObject.SetActive(true);
		}

		private void GoldCheck()
		{
			if (CurrentBonus == null || currentButton == null) return;

			var num = CurrentBonus.GetCost();
			currentButton.Enable(player.Gold.CurrentValue >= num);
		}

		private void OnBonusChanged()
		{
			var num = CurrentBonus.GetCost();
			currentButton.Enable(player.Gold.CurrentValue >= num);
			currentButton.SetText(num.ToStringPritty());
		}

		private void OnBuyClick()
		{
			onBuyClick?.Invoke(this);
		}

		private void OnLocalizationChanged()
		{
			if (CurrentBonus == null) return;

			Title.text = information.isNameId ? localizationSystem.Translate(information.name) : information.name;
			Description.text = information.isDescriptionId ? localizationSystem.Translate(information.description) : information.description;
		}

		public class Factory : PlaceholderFactory<UIMarketItem> { }
	}

	public enum BuyType
	{
		None,

		BUY,//once
		GET,//once
		UPGADE,
		LOCK,
	}
}