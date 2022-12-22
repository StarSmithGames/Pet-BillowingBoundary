using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.MarketSystem
{
	public class UIBuyButton : MonoBehaviour
	{
		[field: SerializeField] public Button Button { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }

		private void Start()
		{
			Button.onClick.AddListener(OnClick);
		}

		private void OnDestroy()
		{
			Button?.onClick.RemoveAllListeners();
		}

		public void SetText(string text)
		{
			Text.text = text;
		}

		private void OnClick()
		{

		}
	}
}