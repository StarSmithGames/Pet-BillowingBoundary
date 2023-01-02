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
				adSystem.AdRewarded.onClosed += OnRewardClosed;
			}

			UpdateUI();
		}

		public void UpdateUI()
		{
			totalReward = BFN.Zero;

			if (isCost)
			{
				BFN reward = waveRoad.CurrentWave.CurrentValue <= 3 ? data.baseCost : BFN.FormuleExpoPremiumMarketAddReward(data.baseCost, 0);

				totalReward += reward;
				Reward.text = reward.ToStringPritty();
			}

			if (isAdd)
			{
				BFN add = waveRoad.CurrentWave.CurrentValue <= 3 ? data.baseAdd : BFN.FormuleExpoPremiumMarketFreeReward(data.baseAdd, 0);

				totalReward += add;
				AddReward.text = $"+{add.ToStringPritty()} Free";
			}

			if (!isFree)
			{
				Price.text = iapManager.GetProducePriceFromStore(key);
			}
		}

		private void onPurchased(string id, bool trigger)
		{
			ButtonReward.interactable = true;
			
			if(string.Equals(id, key, StringComparison.Ordinal))//iap
			{
				floatingAwards.StartAwardCoins(ButtonReward.transform.position, totalReward);
			}
		}

		private void OnRewardClosed(RewardedClosedType closedType)
		{
			ButtonReward.interactable = true;
			
			if (closedType == RewardedClosedType.Rewarded)//free
			{
				floatingAwards.StartAwardCoins(ButtonReward.transform.position, totalReward);
			}
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