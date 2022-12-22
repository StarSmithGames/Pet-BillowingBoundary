using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Game.Systems.FloatingSystem
{
    public class FloatingCoin2D : FloatingObject
	{
		public CanvasGroup canvasGroup;

		public override Tween Fade(float endValue, float duration)
		{
			return canvasGroup.DOFade(endValue, duration);
		}

		public override void SetFade(float endValue)
		{
			canvasGroup.alpha = endValue;
		}

		public class Factory : PlaceholderFactory<FloatingCoin2D> { }
	}
}