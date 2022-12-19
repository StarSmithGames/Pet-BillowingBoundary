using UnityEngine;

using Zenject;

namespace Game.Systems.FloatingTextSystem
{
	public class FloatingText : PoolableObject
	{
		[field: SerializeField] public TMPro.TextMeshPro Text { get; private set; }

		public class Factory : PlaceholderFactory<FloatingText> { }
	}
}