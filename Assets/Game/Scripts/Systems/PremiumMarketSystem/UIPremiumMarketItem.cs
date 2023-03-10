using Game.Managers.AudioManager;
using Game.Managers.IAPManager;
using Game.Managers.StorageManager;
using Game.Managers.VibrationManager;
using Game.Systems.AdSystem;

using Sirenix.OdinInspector;

using System;
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
		private BFN totalReward;

		private AdSystem.AdSystem adSystem;
		private IAPManager iapManager;
		private ISaveLoad saveLoad;
		private AudioManager audioManager;
		private VibrationManager vibrationManager;
		private WaveRoadSystem.WaveRoad waveRoad;
		private FloatingSystem.FloatingAwards floatingAwards;

		[Inject]
		private void Construct(
			AdSystem.AdSystem adSystem,
			IAPManager iapManager,
			ISaveLoad saveLoad,
			AudioManager audioManager,
			VibrationManager vibrationManager,
			WaveRoadSystem.WaveRoad waveRoad,
			FloatingSystem.FloatingAwards floatingAwards)
		{
			this.adSystem = adSystem;
			this.iapManager = iapManager;
			this.saveLoad = saveLoad;
			this.audioManager = audioManager;
			this.vibrationManager = vibrationManager;
			this.waveRoad = waveRoad;
			this.floatingAwards = floatingAwards;
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
				adSystem.AdRewarded.onRewardedClosed += OnRewardedClosed;
				adSystem.AdRewarded.onRewardClosed += OnRewardClosed;
			}

			UpdateUI();
		}

		public void UpdateUI()
		{
			totalReward = BFN.Zero;

			if (isCost)
			{
				BFN reward = isFree ? BFN.FormuleExpoPremiumMarketFreeReward(data.baseCost, waveRoad.CurrentWave.CurrentValue) : BFN.FormuleExpoPremiumMarketReward(data.baseCost, waveRoad.CurrentWave.CurrentValue);

				totalReward += reward;
				Reward.text = reward.ToStringPritty();
			}

			if (isAdd)
			{
				BFN add = BFN.FormuleExpoPremiumMarketAddReward(data.baseAdd, waveRoad.CurrentWave.CurrentValue);

				totalReward += add;
				AddReward.text = $"+{add.ToStringPritty()} Free";
			}

			if (!isFree)
			{
				Price.text = iapManager.GetProducePriceFromStore(key);
			}

			ButtonReward.interactable = true;
		}

		private void onPurchased(string id, bool trigger)
		{
			ButtonReward.interactable = true;
			
			if(string.Equals(id, key, StringComparison.Ordinal))//iap
			{
				floatingAwards.StartAwardCoins(ButtonReward.transform.position, totalReward);
			}
		}

		private void OnRewardedClosed()
		{
			ButtonReward.interactable = true;

			floatingAwards.StartAwardCoins(ButtonReward.transform.position, totalReward);
		}

		private void OnRewardClosed()
		{
			ButtonReward.interactable = true;
		}

		private void OnClick()
		{
			ButtonReward.interactable = false;

			audioManager.PlayButtonClick();
			vibrationManager.Vibrate();

#if UNITY_EDITOR
			ButtonReward.interactable = true;
#endif

			if (data.type == PremiumItemType.ADS)
			{
				if (!adSystem.AdRewarded.Show())
				{
					//something went wrong 
					ButtonReward.interactable = true;
				}
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