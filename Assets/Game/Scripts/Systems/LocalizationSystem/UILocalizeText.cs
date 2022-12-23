using UnityEngine;

using Zenject;

namespace Game.Systems.LocalizationSystem
{
	public class UILocalizeText : MonoBehaviour
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }
		[SerializeField] private string localizationId = "";

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
			localizationId = localizationId.ToLowerInvariant();
			signalBus?.Subscribe<SignalLocalizationChanged>(OnLocalizationChanged);
			OnLocalizationChanged();
		}

		private void OnDestroy()
		{
			signalBus?.TryUnsubscribe<SignalLocalizationChanged>(OnLocalizationChanged);
		}

		private void OnLocalizationChanged()
		{
			if (localizationId.IsEmpty()) return;

			Text.text = localizationSystem.Translate(localizationId);
		}
	}
}