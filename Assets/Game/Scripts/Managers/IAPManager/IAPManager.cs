using UnityEngine;
using UnityEngine.Purchasing;
using System;

using Zenject;
using Game.Systems.AdSystem;
using UnityEngine.Events;
using Game.Managers.StorageManager;
using Game.Systems.AnalyticsSystem;

namespace Game.Managers.IAPManager
{
	public sealed class IAPManager : MonoBehaviour, IStoreListener
	{
		public event UnityAction<bool> onPurchased;

		public readonly string removeADS = "remove_ads";
		public readonly string freeMode = "free_mode";
		public readonly string item1 = "item_1";
		public readonly string item2 = "item_2";
		public readonly string item3 = "item_3";
		public readonly string item4 = "item_4";

		private bool isInitialized = false;
		private IStoreController storeController;
		private IExtensionProvider storeExtensionProvider;

		private ISaveLoad saveLoad;
		private AnalyticsSystem analyticsSystem;
		private AdSystem adSystem;

		[Inject]
		private void Construct(ISaveLoad saveLoad, AnalyticsSystem analyticsSystem, AdSystem adSystem)
		{
			this.saveLoad = saveLoad;
			this.analyticsSystem = analyticsSystem;
			this.adSystem = adSystem;
		}

		private void Start()
		{
			var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

			builder.AddProduct(removeADS, ProductType.NonConsumable);
			builder.AddProduct(freeMode, ProductType.NonConsumable);
			builder.AddProduct(item1, ProductType.Consumable);
			builder.AddProduct(item2, ProductType.Consumable);
			builder.AddProduct(item3, ProductType.Consumable);
			builder.AddProduct(item4, ProductType.Consumable);

			//, new IDs
			//			{
			//				{"100_gold_coins_google", GooglePlay.Name},
			//				{"100_gold_coins_mac", MacAppStore.Name}
			//			}
			UnityPurchasing.Initialize(this, builder);
		}

		public string GetProducePriceFromStore(string productId) => isInitialized ? storeController.products.WithID(productId).metadata.localizedPriceString : "";

		public void BuyProductID(string productId)
		{
			if (!isInitialized) return;

			Product product = storeController.products.WithID(productId);

			if (product != null && product.availableToPurchase)
			{
				Debug.Log($"[IAPManager] Purchasing product asychronously: '{product.definition.id}'");

				storeController.InitiatePurchase(product);
			}
			else
			{
				Debug.Log($"[IAPManager] BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase {(product != null)} {(product?.availableToPurchase)}");
			}
		}


		/// <summary>
		/// apple Only
		/// </summary>
		public void RestorePurchases()
		{
			if (!isInitialized) return;

			if (Application.platform == RuntimePlatform.IPhonePlayer ||
				Application.platform == RuntimePlatform.OSXPlayer)
			{
				var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
				apple.RestoreTransactions((result) =>
				{
					Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
				});
			}
		}

		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
		{
			string id = args.purchasedProduct.definition.id;

			if (string.Equals(id, removeADS, StringComparison.Ordinal))
			{
				saveLoad.GetStorage().IsBuyRemoveADS.SetData(true);

				adSystem.Enable(false);

				analyticsSystem.LogEvent_iap_remove_ads();

				onPurchased?.Invoke(true);
			}
			else if (string.Equals(id, freeMode, StringComparison.Ordinal))
			{
				saveLoad.GetStorage().IsBuyFreeMode.SetData(true);

				adSystem.Enable(false);

				analyticsSystem.LogEvent_iap_free_mode();

				onPurchased?.Invoke(true);
			}
			else if (string.Equals(id, item1, StringComparison.Ordinal))
			{
				analyticsSystem.LogEvent_iap_buy(1);

				onPurchased?.Invoke(true);
			}
			else if (string.Equals(id, item2, StringComparison.Ordinal))
			{
				analyticsSystem.LogEvent_iap_buy(2);

				onPurchased?.Invoke(true);
			}
			else if (string.Equals(id, item3, StringComparison.Ordinal))
			{
				analyticsSystem.LogEvent_iap_buy(3);
				
				onPurchased?.Invoke(true);
			}
			else if (string.Equals(id, item4, StringComparison.Ordinal))
			{
				analyticsSystem.LogEvent_iap_buy(4);

				onPurchased?.Invoke(true);
			}
			else
			{
				Debug.LogError($"[IAPManager] ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
				onPurchased?.Invoke(false);
			}

			saveLoad.GetStorage().IsPayUser.SetData(true);

			return PurchaseProcessingResult.Complete;
		}

		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		{
			storeController = controller;
			storeExtensionProvider = extensions;

			isInitialized = true;

			Debug.Log("[IAPManager] Initialized.");
		}

		public void OnInitializeFailed(InitializationFailureReason error)
		{
			isInitialized = false;

			Debug.Log($"[IAPManager] InitializationFailureReason {error}.");
		}

		public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
		{
			Debug.Log($"[IAPManager] OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");

			onPurchased?.Invoke(false);
		}
	}
}