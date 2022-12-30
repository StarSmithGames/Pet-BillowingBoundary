using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Managers.AudioManager
{
	public class AudioSource : PoolableObject
	{
		[SerializeField] private UnityEngine.AudioSource source;

		public bool isPlaying { get; private set; } = false;

		private float t = 0;
		private float playTime = 0;

		private void Update()
		{
			if (isPlaying)
			{
				t += Time.deltaTime;
				
				if(t >= playTime)
				{
					isPlaying = false;
					t = 0;
					OnPlayingChanged(isPlaying);
				}
			}
		}

		public void PlayOnce(AudioClip clip)
		{
			playTime = clip.length;
			source.PlayOneShot(clip);
			isPlaying = true;

			OnPlayingChanged(isPlaying);
		}

		private void OnPlayingChanged(bool trigger)
		{
			if(trigger == false)
			{
				DespawnIt();
			}
		}

		public class Factory : PlaceholderFactory<AudioSource> { }
	}
}