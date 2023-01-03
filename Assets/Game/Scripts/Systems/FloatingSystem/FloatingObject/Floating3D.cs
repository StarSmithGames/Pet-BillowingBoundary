using DG.Tweening;

using Sirenix.OdinInspector;

using System;
using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Game.Systems.FloatingSystem
{
	public abstract class Floating3D : FloatingObject
	{
		[SerializeField] private bool isSingleModel = false;
		[ShowIf("isSingleModel")]
		[SerializeField] private FloatingModel model;
		[HideIf("isSingleModel")]
		[SerializeField] private List<FloatingModel> models = new List<FloatingModel>();
		[HideIf("isSingleModel")]
		[SerializeField] private int index = 0;
		public Rigidbody Rigidbody => rigidbody;
		[SerializeField] private Rigidbody rigidbody;

		public FloatingModel CurrentModel { get; private set; }

		private Vector3 lastVelocity;
		private Vector3 currentDirection;
		private bool isDirectionChanged = false;

		private CameraSystem.CameraSystem cameraSystem;

		[Inject]
		private void Construct(CameraSystem.CameraSystem cameraSystem)
		{
			this.cameraSystem = cameraSystem;
		}

		private void Awake()
		{
			if (isSingleModel)
			{
				CurrentModel = model;
			}
			else
			{
				for (int i = 0; i < models.Count; i++)
				{
					models[i].Enable(false);
				}

				CurrentModel = models[index];
			}
		}

		private void OnEnable()
		{
			rigidbody.velocity = Vector3.zero;

			if (!isSingleModel)
			{
				CurrentModel?.Enable(false);
				CurrentModel = models.RandomItem();
			}

			CurrentModel.Show();
			CurrentModel.StartRotate();
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

		public override void SetFade(float endValue) { }

		public override Tween Fade(float endValue, float duration)
		{
			return null;
		}


		public override void DespawnIt()
		{
			CurrentModel.Hide(base.DespawnIt);
		}

		public override void OnDespawned()
		{
			CurrentModel.Kill();
			base.OnDespawned();
		}


		[Button(DirtyOnClick = true)]
		private void Next()
		{
			if (isSingleModel) return;

			for (int i = 0; i < models.Count; i++)
			{
				models[i].Enable(false);
			}

			index = (index + 1) % models.Count;
			models[index].Enable(true);
		}
	}
}