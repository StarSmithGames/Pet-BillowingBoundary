using Game.Entities;
using Game.Managers.StorageManager;
using Game.Systems.LocalizationSystem;
using Game.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public interface IPurchasable
	{
		bool IsUnknow { get; }
		BuyType BuyType { get; }

		Information Information { get; }

		string GetName(bool isRich = true);
		string GetDescription(bool isRich = true);
		BFN GetCost();
		void Purchase();
	}


	public class UIMarketItem : PoolableObject
	{
		public UnityAction<UIMarketItem> onBuyClick;

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

		public IPurchasable CurrentPurchase { get; private set; }

		protected UIBuyButton currentButton;

		protected SignalBus signalBus;
		protected Player player;
		protected ISaveLoad saveLoad;
		protected MarketHandler marketHandler;
		protected FastMessageWindow.Factory fastMessagesFactory;

		[Inject]
		private void Construct(SignalBus signalBus,
			Player player,
			ISaveLoad saveLoad,
			MarketHandler marketHandler,
			FastMessageWindow.Factory fastMessagesFactory)
		{
			this.signalBus = signalBus;
			this.player = player;
			this.saveLoad = saveLoad;
			this.marketHandler = marketHandler;
			this.fastMessagesFactory = fastMessagesFactory;
		}

		protected virtual void Start()
		{
			Buy.Button.onClick.AddListener(OnBuyClick);
			Get.Button.onClick.AddListener(OnBuyClick);
			Upgrade.Button.onClick.AddListener(OnBuyClick);
			Lock.Button.onClick.AddListener(OnLockClick);

			player.Gold.onChanged += GoldCheck;
			saveLoad.GetStorage().IsBuyFreeMode.onChanged += GoldCheck;

			signalBus?.Subscribe<SignalLocalizationChanged>(OnLocalizationChanged);
		}

		protected virtual void OnDestroy()
		{
			Buy?.Button.onClick.RemoveAllListeners();
			Get?.Button.onClick.RemoveAllListeners();
			Upgrade?.Button.onClick.RemoveAllListeners();
			Lock?.Button.onClick.RemoveAllListeners();

			if (player != null)
			{
				player.Gold.onChanged -= GoldCheck;
			}

			saveLoad.GetStorage().IsBuyFreeMode.onChanged -= GoldCheck;

			signalBus?.TryUnsubscribe<SignalLocalizationChanged>(OnLocalizationChanged);
		}

		public virtual void SetPurchasing(IPurchasable purchasable)
		{
			CurrentPurchase = purchasable;
		}

		protected void SetState(BuyType type)
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
			if (CurrentPurchase == null || currentButton == null) return;

			if (saveLoad.GetStorage().IsBuyFreeMode.GetData())
			{
				currentButton.Enable(true);
				currentButton?.SetText(BFN.Zero.ToStringPritty());
			}
			else
			{
				currentButton.Enable(marketHandler.IsPlayerCanBuy(CurrentPurchase));
			}
		}

		private void OnBuyClick()
		{
			onBuyClick?.Invoke(this);
		}

		protected virtual void OnLockClick() { }

		protected void OnPurchasableChanged(IPurchasable purchasable)
		{
			//Icon
			if (purchasable.IsUnknow)
			{
				IconSimple.Enable(false);
				IconFull.Enable(false);
				IconUnknow.Enable(true);

				SetState(BuyType.None);
			}
			else
			{
				if (purchasable.Information.isIconSimple)
				{
					IconSimple.Icon.sprite = CurrentPurchase.Information.portrait;
					IconSimple.Shadow.sprite = IconSimple.Icon.sprite;
				}
				else
				{
					IconFull.Icon.sprite = CurrentPurchase.Information.portrait;
				}

				IconSimple.Enable(purchasable.Information.isIconSimple);
				IconFull.Enable(!purchasable.Information.isIconSimple);
				IconUnknow.Enable(false);

				SetState(purchasable.BuyType);

				if (saveLoad.GetStorage().IsBuyFreeMode.GetData())
				{
					currentButton?.Enable(true);
					currentButton?.SetText(BFN.Zero.ToStringPritty());
				}
				else
				{
					currentButton?.Enable(marketHandler.IsPlayerCanBuy(purchasable));
					currentButton?.SetText(purchasable.GetCost().ToStringPritty());
				}
			}

			OnLocalizationChanged();
		}

		protected void OnLocalizationChanged()
		{
			if (CurrentPurchase == null) return;

			if (CurrentPurchase.IsUnknow)
			{
				Title.text = CurrentPurchase.GetName(false).ReplaceAllCharsOn('?');
				Description.text = CurrentPurchase.GetDescription().ReplaceAllCharsOn('?');
			}
			else
			{
				Title.text = CurrentPurchase.GetName();
				Description.text = CurrentPurchase.GetDescription();
			}
		}
	}

	public enum BuyType
	{
		None,

		BUY,
		GET,//once
		UPGADE,
		LOCK,
	}
}