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
	public class UIMarketBonusItem : PoolableObject
	{
		public UnityAction<UIMarketBonusItem> onBuyClick;

		[field: SerializeField] public TMPro.TextMeshProUGUI Title { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Description { get; private set; }
		[field: Space]
		[field: SerializeField] public UIBuyButton Buy { get; private set; }
		[field: SerializeField] public UIBuyButton Get { get; private set; }
		[field: SerializeField] public UIBuyButton Upgrade { get; private set; }
		[field: SerializeField] public UIBuyButton Lock { get; private set; }
		[field: Space]
		[field: SerializeField] public UIIcon IconSimple { get; private set; }
		[field: SerializeField] public UIIcon IconFull { get; private set; }
		[field: SerializeField] public UIIcon IconUnknow { get; private set; }
		[field: Space]
		[field: SerializeField] public GameObject Separator { get; private set; }

		public Bonus CurrentBonus { get; private set; }

		private UIBuyButton currentButton;

		private SignalBus signalBus;
		private Player player;
		private MarketHandler marketHandler;

		[Inject]
		private void Construct(SignalBus signalBus, Player player, MarketHandler marketHandler)
		{
			this.signalBus = signalBus;
			this.player = player;
			this.marketHandler = marketHandler;
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

			signalBus?.TryUnsubscribe<SignalLocalizationChanged>(OnLocalizationChanged);
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

				OnBonusChanged(CurrentBonus);
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
			Lock.gameObject.SetActive(false);

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
					currentButton = Lock;
					break;
				}
			}

			currentButton?.gameObject.SetActive(true);
		}

		private void GoldCheck()
		{
			if (CurrentBonus == null || currentButton == null) return;

			currentButton.Enable(marketHandler.IsPlayerCanBuy(CurrentBonus));
		}

		private void OnBonusChanged(Bonus bonus)
		{
			//Icon
			if (bonus.IsUnknow)
			{
				IconSimple.Enable(false);
				IconFull.Enable(false);
				IconUnknow.Enable(true);

				SetState(BuyType.None);
			}
			else
			{
				if (bonus.BonusData.isIconSimple)
				{
					IconSimple.Icon.sprite = CurrentBonus.BonusData.information.portrait;
					IconSimple.Shadow.sprite = IconSimple.Icon.sprite;
				}
				else
				{
					IconFull.Icon.sprite = CurrentBonus.BonusData.information.portrait;
				}

				IconSimple.Enable(bonus.BonusData.isIconSimple);
				IconFull.Enable(!bonus.BonusData.isIconSimple);
				IconUnknow.Enable(false);

				SetState(bonus.BuyType);

				currentButton.Enable(marketHandler.IsPlayerCanBuy(bonus));
				currentButton.SetText(bonus.GetCost().ToStringPritty());
			}

			OnLocalizationChanged();
		}

		private void OnBuyClick()
		{
			onBuyClick?.Invoke(this);
		}

		private void OnLocalizationChanged()
		{
			if (CurrentBonus == null) return;

			if (CurrentBonus.IsUnknow)
			{
				Title.text = CurrentBonus.GetName().ReplaceAllCharsOn('?');
				Description.text = CurrentBonus.GetDescription().ReplaceAllCharsOn('?');
			}
			else
			{
				Title.text = CurrentBonus.GetName();
				Description.text = CurrentBonus.GetDescription();
			}
		}

		public class Factory : PlaceholderFactory<UIMarketBonusItem> { }
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