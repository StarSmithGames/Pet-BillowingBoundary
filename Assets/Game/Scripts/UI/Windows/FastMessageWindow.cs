using DG.Tweening;

using Game.Systems.LocalizationSystem;

using UnityEditor.PackageManager.UI;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

namespace Game.UI
{
    public class FastMessageWindow : WindowPopupBasePoolable
	{
		private UnityAction<FastMessageResult> result;

		[field: SerializeField] public Button Close { get; private set; }
		[field: SerializeField] public Button Blank { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Title { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Message { get; private set; }

		private SignalBus signalBus;
		private LocalizationSystem localizationSystem;

		[Inject]
		private void Construct(SignalBus signalBus, LocalizationSystem localizationSystem)
		{
			this.signalBus = signalBus;
			this.localizationSystem = localizationSystem;
		}

		private void Start()
		{
			Close.onClick.AddListener(OnClick);
			Blank.onClick.AddListener(OnClick);
		}

		private void OnDestroy()
		{
			Close.onClick.RemoveAllListeners();
			Blank.onClick.RemoveAllListeners();
		}

		public void Show(FastMessageType type, UnityAction<FastMessageResult> callback = null)
		{
			result = callback;

			if (type == FastMessageType.LockedBonus)
			{
				Title.text = localizationSystem.Translate("ui.fast_message_window.locked_bonus.title");
				Message.text = localizationSystem.Translate("ui.fast_message_window.locked_bonus.message");
			}

			base.Show();
		}


		public override void OnSpawned(IMemoryPool pool)
		{
			base.OnSpawned(pool);

			transform.SetAsLastSibling();
		}

		private void OnClick()
		{
			result?.Invoke(FastMessageResult.Closed);
			base.Hide();
		}

		public class Factory : PlaceholderFactory<FastMessageWindow> { }
	}

	public enum FastMessageType
	{
		LockedBonus,
	}

	public enum FastMessageResult
	{
		Closed,
	}
}