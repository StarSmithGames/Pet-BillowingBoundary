using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.MarketSystem
{
	public class UIMarketItem : MonoBehaviour
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Count { get; private set; }
		[field: SerializeField] public Button Button { get; private set; }

		private void Start()
		{
			Button.onClick.AddListener(OnClick);
		}

		private void OnDestroy()
		{
			Button?.onClick.RemoveAllListeners();
		}

		private void OnClick()
		{

		}
	}
}