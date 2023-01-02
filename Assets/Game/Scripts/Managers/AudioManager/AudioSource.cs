using Unity.VisualScripting;

using UnityEngine;
using Zenject;

namespace Game.Managers.AudioManager
{
	[RequireComponent(typeof(UnityEngine.AudioSource))]
	public class AudioSource : PoolableObject
	{
		private UnityEngine.AudioSource Source
		{
			get
			{
				if(source == null)
				{
					source = gameObject.AddComponent<UnityEngine.AudioSource>();
				}

				return source;
			}
		}
		private UnityEngine.AudioSource source;

		public bool isPlaying { get; private set; } = false;

		private bool isLoop = false;

		private float t = 0;
		private float playTime = 0;

		private void Update()
		{
			if (isLoop) return;

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

		public void Mute(bool trigger)
		{
			Source.mute = trigger;
		}

		public void PlayLoop(AudioClip clip)
		{
			Source.loop = true;

			Source.clip = clip;
			Source.Play();

			isLoop = true;
		}

		public void PlayOnce(AudioClip clip)
		{
			isLoop = false;

			Source.loop = false;

			playTime = clip.length;
			Source.PlayOneShot(clip);
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