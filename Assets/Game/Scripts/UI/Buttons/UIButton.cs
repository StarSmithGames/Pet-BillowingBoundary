using Game.Managers.AudioManager;
using Game.Managers.VibrationManager;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
	public class UIButton : MonoBehaviour
	{
		public bool IsEnable { get; private set; } = true;

		[field: SerializeField] public Button Button { get; private set; }

		protected VibrationManager vibrationManager;
		protected AudioManager audioManager;

		[Inject]
		private void Construct(AudioManager audioManager, VibrationManager vibrationManager)
		{
			this.audioManager = audioManager;
			this.vibrationManager = vibrationManager;
		}

		protected virtual void Start()
		{
			Button.onClick.AddListener(OnClick);
		}

		protected virtual void OnDestroy()
		{
			Button.onClick.RemoveAllListeners();
		}

		public virtual void Enable(bool trigger)
		{
			IsEnable = trigger;
		}

		protected virtual void OnClick()
		{
			audioManager.PlayButtonClick();
			vibrationManager.Vibrate();
		}
	}
}