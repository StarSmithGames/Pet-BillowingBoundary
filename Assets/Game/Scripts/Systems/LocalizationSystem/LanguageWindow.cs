using DG.Tweening;
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

		private UISubCanvas subCanvas;
		private LocalizationSystem localizationSystem;

		[Inject]
		private void Construct(UISubCanvas subCanvas, LocalizationSystem localizationSystem)
		{
			this.subCanvas = subCanvas;
			this.localizationSystem = localizationSystem;
		}

		private void Start()
		{
			Enable(false);

			Blank.onClick.AddListener(OnClosed);
			Close.onClick.AddListener(OnClosed);

			subCanvas.WindowsRegistrator.Registrate(this);

			var names = localizationSystem.GetAllLanguageNativeNames();
			var index = localizationSystem.CurrentLocaleIndex;
			for (int i = 0; i < langs.Count; i++)
			{
				langs[i].SetText(names[i]);
				langs[i].Enable(i == index);
				langs[i].onClicked += OnLangClicked;
			}
		}

		private void OnDestroy()
		{
			Blank?.onClick.RemoveAllListeners();
			Close?.onClick.RemoveAllListeners();

			for (int i = 0; i < langs.Count; i++)
			{
				langs[i].onClicked -= OnLangClicked;
			}

			subCanvas.WindowsRegistrator.UnRegistrate(this);
		}

		private void OnLangClicked(UILanguageButton lang)
		{
			if (localizationSystem.IsLocaleProcess) return;

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
		}

		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			langs = GetComponentsInChildren<UILanguageButton>(true).ToList();
		}
	}
}