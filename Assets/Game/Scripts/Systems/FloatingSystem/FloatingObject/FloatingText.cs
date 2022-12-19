using DG.Tweening;

using UnityEngine;

using Zenject;

namespace Game.Systems.FloatingSystem
{
	public class FloatingText : FloatingObject
	{
		[field: SerializeField] public TMPro.TextMeshPro Text { get; private set; }

		public override void SetFade(float endValue)
		{
			Text.DOFade(endValue, 0.01f);
		}

		public override Tween Fade(float endValue, float duration)
		{
			return Text.DOFade(endValue, duration);
		}

		public class Factory : PlaceholderFactory<FloatingText> { }
	}
}