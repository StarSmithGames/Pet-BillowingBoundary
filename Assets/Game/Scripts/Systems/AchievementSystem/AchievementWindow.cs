using DG.Tweening;

using Game.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.AchievementSystem
{
	public class AchievementWindow : WindowBase
	{
		[field: SerializeField] public Button Open { get; private set; }
		[field: SerializeField] public Button Close { get; private set; }
		[field: SerializeField] public Transform Content { get; private set; }
		[field: SerializeField] public RectTransform Window { get; private set; }

		private UISubCanvas subCanvas;

		[Inject]
		private void Construct(UISubCanvas subCanvas)
		{
			this.subCanvas = subCanvas;
		}

		private void Start()
		{
			Enable(false);

			Open.onClick.AddListener(OnOpened);
			Close.onClick.AddListener(OnClosed);

			subCanvas.WindowsRegistrator.Registrate(this);
		}

		private void OnDestroy()
		{
			Open?.onClick.RemoveAllListeners();
			Close?.onClick.RemoveAllListeners();

			subCanvas.WindowsRegistrator.UnRegistrate(this);
		}

		public override void Show(UnityAction callback = null)
		{
			IsInProcess = true;
			CanvasGroup.alpha = 0f;
			CanvasGroup.Enable(true, false);
			//Window.anchoredPosition = new Vector2(Window.anchoredPosition.x, Window.sizeDelta.y / 2);//up
			IsShowing = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(1f, 0.2f))
				.Join(Window.DOAnchorPos(Vector2.zero, 0.2f))
				.AppendCallback(() =>
				{
					callback?.Invoke();
					IsInProcess = false;
				});
		}

		public override void Hide(UnityAction callback = null)
		{
			IsInProcess = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(0f, 0.15f))
				.Join(Window.DOAnchorPos(new Vector2(0, 1920), 0.15f))
				.AppendCallback(() =>
				{
					CanvasGroup.Enable(false);
					IsShowing = false;
					callback?.Invoke();

					IsInProcess = false;
				});
		}

		private void OnOpened()
		{
			if (IsInProcess) return;

			Show();
		}

		private void OnClosed()
		{
			if (IsInProcess) return;

			Hide();
		}
	}
}