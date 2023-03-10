using DG.Tweening;

using Game.Managers.AudioManager;
using Game.Managers.VibrationManager;
using Game.UI;

using Sirenix.OdinInspector;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.LocalizationSystem
{
    public class LanguageWindow : WindowPopupBase
    {
		[field: SerializeField] public Button Blank { get; private set; }
		[field: SerializeField] public Button Close { get; private set; }
		[Space]
		[SerializeField] private List<UILanguageButton> langs = new List<UILanguageButton>();

		private bool isChangeManualy = false;

		private SignalBus signalBus;
		private UISubCanvas subCanvas;
		private LocalizationSystem localizationSystem;
		private AudioManager audioManager;
		private VibrationManager vibrationManager;
		private AnalyticsSystem.AnalyticsSystem analyticsSystem;

		[Inject]
		private void Construct(
			SignalBus signalBus,
			UISubCanvas subCanvas,
			LocalizationSystem localizationSystem,
			AudioManager audioManager,
			VibrationManager vibrationManager,
			AnalyticsSystem.AnalyticsSystem analyticsSystem)
		{
			this.signalBus = signalBus;
			this.subCanvas = subCanvas;
			this.localizationSystem = localizationSystem;
			this.audioManager = audioManager;
			this.vibrationManager = vibrationManager;
			this.analyticsSystem = analyticsSystem;
		}

		private void Start()
		{
			Enable(false);

			Blank.onClick.AddListener(OnClosed);
			Close.onClick.AddListener(OnClosed);

			subCanvas.WindowsRegistrator.Registrate(this);

			for (int i = 0; i < langs.Count; i++)
			{
				langs[i].onClicked += OnLangClicked;
			}

			signalBus.Subscribe<SignalLocalizationChanged>(OnLocalizationChanged);
			OnLocalizationChanged();

		}

		private void OnDestroy()
		{
			Blank?.onClick.RemoveAllListeners();
			Close?.onClick.RemoveAllListeners();

			for (int i = 0; i < langs.Count; i++)
			{
				langs[i].onClicked -= OnLangClicked;
			}

			signalBus.Unsubscribe<SignalLocalizationChanged>(OnLocalizationChanged);

			subCanvas.WindowsRegistrator.UnRegistrate(this);
		}


		private void OnLocalizationChanged()
		{
			var names = localizationSystem.GetAllLanguageNativeNames();
			var index = localizationSystem.CurrentLocaleIndex;
			for (int i = 0; i < langs.Count; i++)
			{
				langs[i].SetText(names[i]);
				langs[i].Enable(i == index);
			}

			if (isChangeManualy)
			{
				analyticsSystem.LogEvent_settings_language(localizationSystem.CurrentLocaleCode);
			}

			isChangeManualy = false;
		}

		private void OnLangClicked(UILanguageButton lang)
		{
			if (localizationSystem.IsLocaleProcess) return;

			isChangeManualy = true;

			var index = langs.IndexOf(lang);

			for (int i = 0; i < langs.Count; i++)
			{
				langs[i].Enable(i == index);
			}

			localizationSystem.ChangeLocale(index);
		}

		private void OnClosed()
		{
			Hide();

			audioManager.PlayButtonClick();
			vibrationManager.Vibrate();
		}

		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			langs = GetComponentsInChildren<UILanguageButton>(true).ToList();
		}
	}
}