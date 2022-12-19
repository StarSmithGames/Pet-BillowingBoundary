using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

using Zenject;

namespace Game.Systems.FloatingSystem
{
	public class FloatingCoin3D : FloatingObject
	{
		[HideLabel]
		[SerializeField] private Settings settings;
		[Space]
		[SerializeField] private Transform model;
		[SerializeField] private Renderer renderer;

		private Material material;
		
		[Button(DirtyOnClick = true)]
		public void StartRotate()
		{
			model
				.DORotate(new Vector3(0, 360f, 0), settings.duration, RotateMode.FastBeyond360)
				.SetLoops(-1, LoopType.Restart)
				.SetRelative()
				.SetEase(Ease.Linear);
		}

		public override void SetFade(float endValue)
		{
			if (material == null)
			{
				material = new Material(renderer.sharedMaterial);
				renderer.sharedMaterial = material;
			}

			var color = material.color;
			color.a = endValue;
			material.color = color;
		}

		public override Tween Fade(float endValue, float duration)
		{
			if(material == null)
			{
				material = new Material(renderer.sharedMaterial);
				renderer.sharedMaterial = material;
			}

			return material.DOFade(endValue, duration);
		}

		public override void OnDespawned()
		{
			model.DOKill(true);
			base.OnDespawned();
		}

		

		[System.Serializable]
		public class Settings
		{
			public float duration = 0.5f;
		}

		public class Factory : PlaceholderFactory<FloatingCoin3D> { }
	}
}