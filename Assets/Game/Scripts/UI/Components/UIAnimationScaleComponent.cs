using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Game.UI
{
	public class UIAnimationScaleComponent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField] private Transform targetTransform;
		[SerializeField] private float scaleMultiply = 0.95f;

		private Sequence sequence;
		private Vector3 startScale;
		private Vector3 endScale;

		private void Awake()
		{
			if (targetTransform == null)
			{
				targetTransform = transform;
			}

			startScale = targetTransform.localScale;
			endScale = startScale * scaleMultiply;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			TryForceComplete();

			sequence = DOTween.Sequence();
			sequence
				.Append(targetTransform.DOScale(endScale, 0.1f))
				.SetEase(Ease.Linear);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			TryForceComplete();

			sequence = DOTween.Sequence();
			sequence
				.Append(targetTransform.DOScale(startScale, 0.1f))
				.SetEase(Ease.Linear);
		}

		private void TryForceComplete()
		{
			if (sequence != null)
			{
				sequence.Complete(true);
			}
		}
	}
}