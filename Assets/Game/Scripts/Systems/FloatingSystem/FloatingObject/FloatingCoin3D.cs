using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

using Zenject;

namespace Game.Systems.FloatingSystem
{
	public class FloatingCoin3D : FloatingObject
	{
		public Rigidbody Rigidbody => rigidbody;

		[HideLabel]
		[SerializeField] private Settings settings;
		[Space]
		[SerializeField] private Transform model;
		[SerializeField] private Renderer renderer;
		[SerializeField] private Rigidbody rigidbody;

		private Material material;
		private Vector3 lastVelocity;
		private Vector3 currentDirection;
		private bool isDirectionChanged = false;

		private CameraSystem.CameraSystem cameraSystem;

		[Inject]
		private void Construct(CameraSystem.CameraSystem cameraSystem)
		{
			this.cameraSystem = cameraSystem;
		}

		private void OnEnable()
		{
			rigidbody.velocity = Vector3.zero;
		}

		private void FixedUpdate()
		{
			lastVelocity = rigidbody.velocity;

			var screenPosition = cameraSystem.Camera.WorldToScreenPoint(transform.position);

			if (screenPosition.x > Screen.width && currentDirection != Vector3.left)
			{
				currentDirection = Vector3.left;
				isDirectionChanged = true;
			}
			if (screenPosition.x < 0 && currentDirection != Vector3.right)
			{
				currentDirection = Vector3.right;
				isDirectionChanged = true;
			}

			if (screenPosition.y < 0 && currentDirection != Vector3.down)
			{
				currentDirection = Vector3.down;
				isDirectionChanged = true;
			}
			if (screenPosition.y > Screen.height && currentDirection != Vector3.up)
			{
				currentDirection = Vector3.up;
				isDirectionChanged = true;
			}

			if (isDirectionChanged)
			{
				var speed = lastVelocity.magnitude;
				var direction = Vector3.Reflect(lastVelocity.normalized, currentDirection);

				rigidbody.velocity = direction * Mathf.Max(speed, 5f);

				isDirectionChanged = false;
			}
		}

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