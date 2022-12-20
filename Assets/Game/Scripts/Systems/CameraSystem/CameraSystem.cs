using DG.Tweening;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.CameraSystem
{
	public class CameraSystem : MonoBehaviour
	{
		public Camera Camera => camera;
		[SerializeField] private Camera camera;

		[Header("Shake")]
		[SerializeField] private ShakeSettings smallestShake;
		[SerializeField] private ShakeSettings smallShake;

		public void StartCustomShake(ShakeSettings settings)
		{
			camera.DORewind();
			camera.DOShakePosition(settings.duration, settings.strength, settings.vibrato, settings.randomnes, settings.fadeOut, settings.mode);
		}

		[Button]
		public void StartSmallestShake()
		{
			StartCustomShake(smallestShake);
		}

		[Button]
		private void StartSmallShake()
		{
			StartCustomShake(smallShake);
		}
	}

	[System.Serializable]
	public class ShakeSettings
	{
		public float duration = 3f;
		public float strength = 1f;
		public int vibrato = 10;
		public float randomnes = 90f;
		public bool fadeOut = true;
		public ShakeRandomnessMode mode = ShakeRandomnessMode.Full;

		
	}
}