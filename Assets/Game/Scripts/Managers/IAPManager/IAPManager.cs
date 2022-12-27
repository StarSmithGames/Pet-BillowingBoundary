using UnityEngine;
using UnityEngine.Purchasing;
using System;

using Zenject;

namespace Game.Managers.IAPManager
{
	public class IAPManager : MonoBehaviour, IStoreListener
	{
		public readonly string removeADS = "remove_ads";
		public readonly string buyMillion = "one_milion";

		private bool isInitialized = false;
		private IStoreController storeController;
		private IExtensionProvider storeExtensionProvider;

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
				Debug.Log($"Purchasing product asychronously: '{product.definition.id}'");

				storeController.InitiatePurchase(product);
			}
			else
			{
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}

		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
		{
			if (string.Equals(args.purchasedProduct.definition.id, removeADS, StringComparison.Ordinal))
			{
				Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
			}
			else if (string.Equals(args.purchasedProduct.definition.id, buyMillion, StringComparison.Ordinal))
			{
				Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
			}
			else
			{
				Debug.Log($"ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
			}

			return PurchaseProcessingResult.Complete;
		}

		/// <summary>
		/// Apply Only
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
			Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
		}
	}
}