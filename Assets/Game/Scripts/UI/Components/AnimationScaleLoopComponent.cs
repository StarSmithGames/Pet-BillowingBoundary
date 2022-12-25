using DG.Tweening;
using UnityEngine;

namespace Game.UI
{
	public class AnimationScaleLoopComponent : MonoBehaviour
	{
		[SerializeField] private Vector3 scaleTo;
		[SerializeField] private float duration;

		private void Start()
		{
			transform
				.DOScale(scaleTo, duration)
				.SetEase(Ease.InOutSine)
				.SetLoops(-1, LoopType.Yoyo);
		}
	}
}