using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.VFX
{
	public class ParticleVFX : MonoBehaviour
	{
		[SerializeField] private ParticleSystem particleSystem;

		public void Play()
		{
			particleSystem.Play();
		}
	}
}