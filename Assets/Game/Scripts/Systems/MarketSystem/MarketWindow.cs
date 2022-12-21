using DG.Tweening;
using Game.UI;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public class MarketWindow : WindowBase
	{
		[field: SerializeField] public Button Blank { get; private set; }
		[field: SerializeField] public Button Close { get; private set; }
		[field: SerializeField] public RectTransform WindowUp { get; private set; }
		[field: SerializeField] public RectTransform WindowDown { get; private set; }

		private bool isOpenned = false;

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
			Blank.onClick.AddListener(OnClosed);

			subCanvas.WindowsRegistrator.Registrate(this);
		}

		private void OnDestroy()
		{
			Close?.onClick.RemoveAllListeners();
			Blank?.onClick.RemoveAllListeners();

			subCanvas.WindowsRegistrator.UnRegistrate(this);
		}

		public override void Show(UnityAction callback = null)
		{
			IsInProcess = true;
			isOpenned = true;

			CanvasGroup.alpha = 0f;
			CanvasGroup.Enable(true, false);
			WindowUp.anchoredPosition = new Vector2(WindowUp.anchoredPosition.x, WindowUp.sizeDelta.y / 2);//up
			WindowDown.anchoredPosition = new Vector2(WindowDown.anchoredPosition.x, -(WindowDown.sizeDelta.y / 2));//down
			IsShowing = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(1f, 0.2f))
				.Join(WindowUp.DOAnchorPosY(-(WindowUp.sizeDelta.y / 2), 0.25f, true).SetEase(Ease.OutBounce))
				.Join(WindowDown.DOAnchorPosY(WindowDown.sizeDelta.y / 2, 0.25f))
				.AppendCallback(() =>
				{
					callback?.Invoke();
					IsInProcess = false;
				});
		}

		public override void Hide(UnityAction callback = null)
		{
			IsInProcess = true;
			isOpenned = false;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(0f, 0.15f))
				.Join(WindowUp.DOAnchorPosY(WindowUp.sizeDelta.y / 2, 0.15f))
				.Join(WindowDown.DOAnchorPosY(-(WindowDown.sizeDelta.y / 2), 0.15f))
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