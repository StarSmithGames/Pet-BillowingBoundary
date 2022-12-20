using DG.Tweening;
using Game.UI;
using System.Runtime.CompilerServices;


using UnityEditor.PackageManager.UI;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public class MarketWindow : WindowBase
	{
		[field: SerializeField] public Button Close { get; private set; }
		[field: SerializeField] public RectTransform Window { get; private set; }

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

			subCanvas.WindowsRegistrator.Registrate(this);
		}

		private void OnDestroy()
		{
			Close?.onClick.RemoveAllListeners();

			subCanvas.WindowsRegistrator.UnRegistrate(this);
		}

		public override void Show(UnityAction callback = null)
		{
			IsInProcess = true;
			isOpenned = true;

			CanvasGroup.alpha = 0f;
			CanvasGroup.Enable(true, false);
			Window.anchoredPosition = new Vector2(Window.anchoredPosition.x, Window.sizeDelta.y / 2);//up
			IsShowing = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(1f, 0.2f))
				.Join(Window.DOAnchorPosY(-(Window.sizeDelta.y / 2), 0.25f, true).SetEase(Ease.OutBounce))
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
				.Join(Window.DOAnchorPosY(Window.sizeDelta.y / 2, 0.15f))
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