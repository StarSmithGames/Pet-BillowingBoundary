using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.DailyRewardSystem
{
	public class UIRewardItem : MonoBehaviour
	{
		[field: SerializeField] public Image Icon { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Day { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }
		[field: SerializeField] public Button Claim { get; private set; }
		[field: SerializeField] public GameObject Clear { get; private set; }

		private void Start()
		{
			Claim.onClick.AddListener(OnClaimed);
		}

		private void OnDestroy()
		{
			Claim?.onClick.RemoveAllListeners();
		}

		public void SetClaim(bool trigger)
		{
			Claim.gameObject.SetActive(!trigger);
			Clear.SetActive(trigger);
		}

		private void OnClaimed()
		{
			SetClaim(true);
		}
	}
}