using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.Systems.WaveRoadSystem
{
	public class UIPieceAnimatedBar : PoolableObject
	{
		[field: SerializeField] public RectTransform Rect { get; private set; }

		public void SetWidth(float width)
		{
			Vector2 size = Rect.sizeDelta;
			Rect.sizeDelta = new Vector2(width, size.y);
		}

		public class Factory : PlaceholderFactory<UIPieceAnimatedBar> { }
	}
}