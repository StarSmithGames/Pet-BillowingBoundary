using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.MarketSystem
{
	public class UIBuyButton : MonoBehaviour
	{
		public bool IsEnable { get; private set; } = true;

		[field: SerializeField] public Button Button { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }
		[SerializeField] private Sprite on;
		[SerializeField] private Sprite off;

		public void Enable(bool trigger)
		{
			Button.image.sprite = trigger ? on : off;

			IsEnable = trigger;
		}

		public void SetText(string text)
		{
			Text.text = text;
		}
	}
}