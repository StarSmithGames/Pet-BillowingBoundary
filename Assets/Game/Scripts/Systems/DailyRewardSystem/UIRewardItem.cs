using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Systems.DailyRewardSystem
{
	public class UIRewardItem : MonoBehaviour
	{
		public UnityAction<UIRewardItem> onRewardStateChanged;

		[field: SerializeField] public Image Icon { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Day { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }
		[field: SerializeField] public Button Claim { get; private set; }
		[field: SerializeField] public GameObject Clear { get; private set; }
		public DayType DayType => dayType;
		[Space]
		[SerializeField] private DayType dayType = DayType.Day1;

		public DailyRewardState CurrentState { get; private set; } = DailyRewardState.Open;

		private void Start()
		{
			Day.text = $"DAY {(int)dayType}";

			Claim.onClick.AddListener(OnClaimed);
		}

		private void OnDestroy()
		{
			Claim?.onClick.RemoveAllListeners();
		}

		public void SetState(DailyRewardState rewardState, bool notify = true)
		{
			if(rewardState == DailyRewardState.Open)
			{
				Claim.interactable = true;
				Claim.gameObject.SetActive(true);
				Clear.SetActive(false);
			}
			else if(rewardState == DailyRewardState.Close)
			{
				Claim.interactable = false;
				Claim.gameObject.SetActive(true);
				Clear.SetActive(false);
			}
			else if(rewardState == DailyRewardState.Claimed)
			{
				Claim.gameObject.SetActive(false);
				Clear.SetActive(true);
			}

			CurrentState = rewardState;

			if (notify)
			{
				onRewardStateChanged?.Invoke(this);
			}
		}

		private void OnClaimed()
		{
			SetState(DailyRewardState.Claimed);
		}
	}

	public enum DailyRewardState
	{
		Open,
		Close,

		Claimed,
	}
}