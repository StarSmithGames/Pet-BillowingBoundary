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
	public class IAPManager : MonoBehaviour, IStoreListener
	{
		public event UnityAction onPurchaseFailed;

		public readonly string removeADS = "remove_ads";
		public readonly string buyMillion = "one_milion";

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
			builder.AddProduct(buyMillion, ProductType.Consumable);

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
			if (string.Equals(args.purchasedProduct.definition.id, removeADS, StringComparison.Ordinal))
			{
				adSystem.Enable(false);

				analyticsSystem.LogEvent_iap_remove_ads();

				Debug.Log($"[IAPManager] ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
			}
			else if (string.Equals(args.purchasedProduct.definition.id, buyMillion, StringComparison.Ordinal))
			{
				Debug.Log($"[IAPManager] ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
			}
			else
			{
				Debug.LogError($"[IAPManager] ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
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

			onPurchaseFailed?.Invoke();
		}
	}
}