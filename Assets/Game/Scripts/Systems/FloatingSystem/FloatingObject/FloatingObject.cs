using DG.Tweening;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.FloatingSystem
{
	public abstract class FloatingObject : PoolableObject
	{
		public abstract void SetFade(float endValue);
		public abstract Tween Fade(float endValue, float duration);
	}
}