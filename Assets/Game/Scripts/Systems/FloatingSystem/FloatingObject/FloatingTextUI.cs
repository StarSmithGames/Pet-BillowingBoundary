using DG.Tweening;
using UnityEngine;

using Zenject;

namespace Game.Systems.FloatingSystem
{
	public class FloatingTextUI : FloatingObject
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }

		public override Tween Fade(float endValue, float duration)
		{
			return Text.DOFade(endValue, duration);
		}

		public override void SetFade(float endValue)
		{
			Text.DOFade(endValue, 0.01f);
		}

		public class Factory : PlaceholderFactory<FloatingTextUI> { }
	}
}