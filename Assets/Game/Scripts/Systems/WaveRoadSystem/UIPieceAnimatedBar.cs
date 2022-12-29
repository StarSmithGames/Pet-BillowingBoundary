using UnityEngine;

using Zenject;

namespace Game.Systems.WaveRoadSystem
{
	public class UIPieceAnimatedBar : PoolableObject
	{
		public float FillAmount
		{
			get => Bar.anchorMax.x;
			set
			{
				Bar.anchorMax = new Vector2(value, Bar.anchorMax.y);
			}
		}

		[field: SerializeField] public RectTransform Bar { get; private set; }

		public class Factory : PlaceholderFactory<UIPieceAnimatedBar> { }
	}
}