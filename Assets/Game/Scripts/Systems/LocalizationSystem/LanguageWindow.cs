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
    public class LanguageWindow : WindowBase
    {
		[field: SerializeField] public Button Blank { get; private set; }
		[field: SerializeField] public Button Close { get; private set; }
		[field: SerializeField] public Transform Window { get; private set; }
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
			for (int i = 0; i < langs.Count; i++)
			{
				if(i < names.Length)
				{
					langs[i].SetText(names[i]);
					langs[i].gameObject.SetActive(true);
				}
				else
				{
					langs[i].gameObject.SetActive(false);
				}

				langs[i].Enable(false);
			}

			langs[localizationSystem.CurrentLocaleIndex].Enable(true);
		}

		private void OnDestroy()
		{
			Blank?.onClick.RemoveAllListeners();
			Close?.onClick.RemoveAllListeners();

			subCanvas.WindowsRegistrator.UnRegistrate(this);
		}

		public override void Show(UnityAction callback = null)
		{
			Window.localScale = Vector3.zero;

			IsInProcess = true;
			CanvasGroup.alpha = 0f;
			CanvasGroup.Enable(true, false);
			IsShowing = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(1f, 0.2f))
				.Join(Window.DOScale(1, 0.35f).SetEase(Ease.OutBounce))
				.AppendCallback(() =>
				{
					callback?.Invoke();
					IsInProcess = false;
				});
			
		}
		public override void Hide(UnityAction callback = null)
		{
			Window.localScale = Vector3.one;

			IsInProcess = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(0f, 0.15f))
				.Join(Window.DOScale(0, 0.25f).SetEase(Ease.InBounce))
				.AppendCallback(() =>
				{
					CanvasGroup.Enable(false);
					IsShowing = false;
					callback?.Invoke();

					IsInProcess = false;
				});
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