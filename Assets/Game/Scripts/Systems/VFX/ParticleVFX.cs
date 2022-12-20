using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.VFX
{
	public class ParticleVFX : MonoBehaviour
	{
		[SerializeField] private ParticleSystem particleSystem;

		private void Start()
		{
			Enable(false);
		}

		public void Enable(bool trigger)
		{
			if (trigger)
			{
				particleSystem.Emit(50);
			}
			else
			{
				particleSystem.Stop();
			}
		}

		public void Play()
		{
			Enable(true);
		}
	}
}