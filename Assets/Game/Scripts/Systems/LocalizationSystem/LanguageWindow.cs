using DG.Tweening;

using Game.UI;

using System.Collections.Generic;

using UnityEditor.PackageManager.UI;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.LocalizationSystem
{
    public class LanguageWindow : WindowBase
    {
		[field: SerializeField] public Button Close { get; private set; }
		[field: SerializeField] public Transform Window { get; private set; }
		[SerializeField] private List<Button> buttons = new List<Button>();

		private UISubCanvas subCanvas;

		[Inject]
		private void Construct(UISubCanvas subCanvas)
		{
			this.subCanvas = subCanvas;
		}

		private void Start()
		{
			Enable(false);

			Close.onClick.AddListener(OnClosed);

			subCanvas.WindowsRegistrator.Registrate(this);
		}

		private void OnDestroy()
		{
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
				.Join(Window.DOScale(1, 0.35f).SetEase(Ease.OutQuart))
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
	}
}