using Game.Managers.AudioManager;
using Game.Managers.VibrationManager;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public class UIBuyButton : MonoBehaviour
	{
		public bool IsEnable { get; private set; } = true;

		[field: SerializeField] public Button Button { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }
		[SerializeField] private Sprite on;
		[SerializeField] private Sprite off;

		private AudioManager audioManager;
		private VibrationManager vibrationManager;

		[Inject]
		private void Construct(
			AudioManager audioManager,
			VibrationManager vibrationManager)
		{
			this.audioManager = audioManager;
			this.vibrationManager = vibrationManager;
		}

		private void Start()
		{
			Button.onClick.AddListener(OnClick);
		}

		private void OnDestroy()
		{
			Button?.onClick.RemoveAllListeners();
		}

		public void Enable(bool trigger)
		{
			Button.image.sprite = trigger ? on : off;

			IsEnable = trigger;
		}

		public void SetText(string text)
		{
			if (Text == null) return;

			Text.text = text;
		}

		private void OnClick()
		{
			audioManager.PlayButtonClick();
			vibrationManager.Vibrate();
		}
	}
}