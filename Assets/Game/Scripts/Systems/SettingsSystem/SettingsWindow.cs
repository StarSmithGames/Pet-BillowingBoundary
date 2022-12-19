using DG.Tweening;

using Game.UI;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.SettingsSystem
{
	public class SettingsWindow : WindowBase
	{
		[field: SerializeField] public Button OpenClose { get; private set; }
		[field: SerializeField] public RectTransform Content { get; private set; }

		private bool isOpenned = false;
		private RectTransform rectTransform;

		private UISubCanvas subCanvas;

		[Inject]
		private void Construct(UISubCanvas subCanvas)
		{
			this.subCanvas = subCanvas;
		}

		private void Start()
		{
			Enable(false);

			OpenClose.onClick.AddListener(OnOpenClosed);

			subCanvas.WindowsRegistrator.Registrate(this);
		}

		private void OnDestroy()
		{
			OpenClose?.onClick.RemoveAllListeners();

			subCanvas.WindowsRegistrator.UnRegistrate(this);
		}

		public override void Show(UnityAction callback = null)
		{
			IsInProcess = true;
			CanvasGroup.alpha = 0f;
			CanvasGroup.Enable(true, false);
			Content.anchoredPosition = new Vector2(Content.anchoredPosition.x, Content.sizeDelta.y / 2);//up
			IsShowing = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(1f, 0.2f))
				.Join(Content.DOAnchorPosY(-(Content.sizeDelta.y / 2), 0.2f, true))
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
				.Join(Content.DOAnchorPosY(Content.sizeDelta.y / 2, 0.15f))
				.AppendCallback(() =>
				{
					CanvasGroup.Enable(false);
					IsShowing = false;
					callback?.Invoke();

					IsInProcess = false;
				});
		}


		private void OnOpenClosed()
		{
			if (IsInProcess) return;

			if (isOpenned)
			{
				Hide();
			}
			else
			{
				Show();
			}

			isOpenned = !isOpenned;
		}
	}
}