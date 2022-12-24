using DG.Tweening;
using Game.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Game.HUD
{
	public class UIAlert : ShowHideFadeBehavior
	{
		private void Start()
		{
			Enable(false);	
		}
		
		public override void Show(UnityAction callback = null)
		{
			IsInProcess = true;
			CanvasGroup.alpha = 0f;
			CanvasGroup.Enable(true, false);
			IsShowing = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(1f, 0.2f))
				.Join(transform.DOScale(1f, 0.2f))
				.AppendCallback(() =>
				{
					callback?.Invoke();
					IsInProcess = false;

					IdleAnimation().Play();
				});
		}
		
		public override void Hide(UnityAction callback = null)
		{
			transform.DOKill(true);

			IsInProcess = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(0f, 0.15f))
				.Join(transform.DOScale(0f, 0.15f))
				.AppendCallback(() =>
				{
					CanvasGroup.Enable(false);
					IsShowing = false;
					callback?.Invoke();

					IsInProcess = false;
				});
		}

		private Tween IdleAnimation()
		{
			return transform
				.DOPunchScale(new Vector3(0.25f, 0.25f, 0.25f), 0.2f)
				.SetLoops(-1)
				.SetEase(Ease.OutBounce);
		}
	}
}