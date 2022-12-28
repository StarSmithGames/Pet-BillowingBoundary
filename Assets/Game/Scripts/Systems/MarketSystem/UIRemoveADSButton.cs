using Game.Managers.IAPManager;
using Game.Managers.StorageManager;
using Game.Systems.AdSystem;
using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public class UIRemoveADSButton : UIButton
	{
		[SerializeField] private UIRemoveADSPurchased purchased;

		private SignalBus signalBus;
		private IAPManager iapManager;
		private ISaveLoad saveLoad;

		[Inject]
		private void Construct(SignalBus signalBus, IAPManager iapManager, ISaveLoad saveLoad)
		{
			this.signalBus = signalBus;
			this.iapManager = iapManager;
			this.saveLoad = saveLoad;
		}

		protected override void Start()
		{
			base.Start();

			bool isRemoveADS = saveLoad.GetStorage().IsBuyRemoveADS.GetData();

			if (isRemoveADS)
			{
				Hide();
			}

			iapManager.onPurchaseFailed += OnPurchaseFailed;

			signalBus?.Subscribe<SignalADSEnableChanged>(OnADSEnableChanged);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			iapManager.onPurchaseFailed -= OnPurchaseFailed;

			signalBus?.Unsubscribe<SignalADSEnableChanged>(OnADSEnableChanged);
		}

		private void Hide()
		{
			purchased.gameObject.SetActive(true);
			Button.gameObject.SetActive(false);
		}

		private void OnADSEnableChanged(SignalADSEnableChanged signal)
		{
			if (!signal.trigger)
			{
				Hide();
			}
		}

		private void OnPurchaseFailed()
		{
			Button.interactable = true;
		}

		protected override void OnClick()
		{
			Button.interactable = false;

			iapManager.BuyProductID(iapManager.removeADS);

			base.OnClick();
		}
	}
}