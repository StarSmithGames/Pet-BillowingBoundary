using DG.Tweening;

using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
	public class UIAnimatedButton : UIButton, IShowable//Popup
	{
		public bool IsShowing { get; protected set; }
		public bool IsInProcess { get; protected set; }

		[field: SerializeField] public CanvasGroup CanvasGroup { get; protected set; }
		[field: SerializeField] public Transform Root { get; private set; }

		public override void Enable(bool trigger)
		{
			base.Enable(trigger);

			CanvasGroup.Enable(trigger);
			IsShowing = trigger;
		}

		public virtual void Show(UnityAction callback = null)
		{
			Root.localScale = Vector3.zero;

			IsInProcess = true;
			CanvasGroup.alpha = 0f;
			CanvasGroup.Enable(true, false);
			IsShowing = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(1f, 0.2f))
				.Join(Root.DOScale(1, 0.35f).SetEase(Ease.OutBounce))
				.AppendCallback(() =>
				{
					callback?.Invoke();
					IsInProcess = false;
				});
		}

		public virtual void Hide(UnityAction callback = null)
		{
			Root.localScale = Vector3.one;

			IsInProcess = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(Root.DOScale(0, 0.25f).SetEase(Ease.InBounce))
				.Join(CanvasGroup.DOFade(0f, 0.25f))
				.AppendCallback(() =>
				{
					CanvasGroup.Enable(false);
					IsShowing = false;
					callback?.Invoke();

					IsInProcess = false;
				});
		}
	}
}