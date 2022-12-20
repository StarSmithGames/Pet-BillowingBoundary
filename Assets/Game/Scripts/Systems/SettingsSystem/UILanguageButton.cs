using Game.Systems.LocalizationSystem;
using Game.UI;

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.SettingsSystem
{
	public class UILanguageButton : MonoBehaviour
	{
		[field: SerializeField] public Button Button { get; private set; }

		private UISubCanvas subCanvas;

		[Inject]
		private void Construct(UISubCanvas subCanvas)
		{
			this.subCanvas = subCanvas;
		}

		private void Start()
		{
			Button.onClick.AddListener(OnClick);
		}

		private void OnDestroy()
		{
			Button.onClick.RemoveAllListeners();
		}

		private void OnClick()
		{
			subCanvas.WindowsRegistrator.Hide<SettingsWindow>();
			subCanvas.WindowsRegistrator.Show<LanguageWindow>();
		}
	}
}