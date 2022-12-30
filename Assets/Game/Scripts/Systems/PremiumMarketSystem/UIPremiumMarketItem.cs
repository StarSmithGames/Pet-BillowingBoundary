using Game.Managers.AudioManager;
using Game.Managers.IAPManager;
using Game.Managers.StorageManager;
using Game.Managers.VibrationManager;
using Game.Systems.AdSystem;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.PremiumMarketSystem
{
	public class UIPremiumMarketItem : MonoBehaviour
	{
		[field: ShowIf("isCost")]
		[field: SerializeField] public TMPro.TextMeshProUGUI Reward { get; private set; }
		[field: ShowIf("@isCost && isAdd")]
		[field: SerializeField] public TMPro.TextMeshProUGUI AddReward{ get; private set; }//+NKK Free

		[field: SerializeField] public Button ButtonReward { get; private set; }
		[field: HideIf("isFree")]
		[field: SerializeField] public TMPro.TextMeshProUGUI Price { get; private set; }

		[SerializeField] private bool isCost = true;
		[SerializeField] private bool isFree = false;
		[SerializeField] private bool isAdd = false;

		private string key;
		private PremiumItemData data;

		private AdSystem.AdSystem adSystem;
		private IAPManager iapManager;
		private ISaveLoad saveLoad;
		private AudioManager audioManager;
		private VibrationManager vibrationManager;

		[Inject]
		private void Construct(
			AdSystem.AdSystem adSystem,
			IAPManager iapManager,
			ISaveLoad saveLoad,
			AudioManager audioManager,
			VibrationManager vibrationManager)
		{
			this.adSystem = adSystem;
			this.iapManager = iapManager;
			this.saveLoad = saveLoad;
			this.audioManager = audioManager;
			this.vibrationManager = vibrationManager;
		}

		private void Start()
		{
			ButtonReward.onClick.AddListener(OnClick);

			iapManager.onPurchased += onPurchased;
		}

		private void OnDestroy()
		{
			ButtonReward?.onClick.RemoveAllListeners();

			iapManager.onPurchased -= onPurchased;
		}

		public void SetData(string key, PremiumItemData data)
		{
			this.key = key;
			this.data = data;

			if (data.type == PremiumItemType.ADS)
			{
				adSystem.AdRewarded.onClosed += OnRewardClosed;
			}

			UpdateUI();
		}

		public void UpdateUI()
		{
			if (isCost)
			{
				Reward.text = data.baseCost.ToStringPritty();
			}

			if (isAdd)
			{
				AddReward.text = $"+{data.baseAdd.ToStringPritty()} Free";
			}

			if (!isFree)
			{
				Price.text = iapManager.GetProducePriceFromStore(key);
			}
		}

		private void onPurchased(bool trigger)
		{
			ButtonReward.interactable = true;
		}

		private void OnRewardClosed(RewardedClosedType closedType)
		{
			if(closedType == RewardedClosedType.Rewarded)
			{
				//coins for ads TODO
			}
		}

		private void OnClick()
		{
			ButtonReward.interactable = false;

			audioManager.PlayButtonClick();
			vibrationManager.Vibrate();
			
			if(data.type == PremiumItemType.ADS)
			{
#if UNITY_EDITOR
				ButtonReward.interactable = true;
#endif
				adSystem.AdRewarded.Show();
			}
			else
			{
				iapManager.BuyProductID(key);
			}
		}
	}

	public enum PremiumItemType
	{
		ADS,
		Simple,
		FreeMode,
	}
}