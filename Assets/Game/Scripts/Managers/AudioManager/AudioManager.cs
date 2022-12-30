using Game.Managers.StorageManager;
using UnityEngine;

using Zenject;

namespace Game.Managers.AudioManager
{
    public class AudioManager : IInitializable
    {
		private AudioSettings settings;
		private AudioSource.Factory audioFactory;
		private ISaveLoad saveLoad;

		public AudioManager(AudioSettings settings, AudioSource.Factory audioFactory, ISaveLoad saveLoad)
        {
			this.settings = settings;
			this.audioFactory = audioFactory;
			this.saveLoad = saveLoad;
		}

		public void Initialize()
		{
			PlayBackground();
		}

		public void PlayBackground()
		{
			if (!saveLoad.GetStorage().IsMusic.GetData()) return;

		}

		public void PlayButtonClick()
		{
			PlaySoundOnce(settings.buttonClick);
		}

		public void PlayBossDefeated()
		{
			PlaySoundOnce(settings.bossDefeated);
		}

		private void PlaySoundOnce(AudioClip clip)
		{
			if (!saveLoad.GetStorage().IsSound.GetData()) return;

			var source = audioFactory.Create();
			source.PlayOnce(clip);
		}
	}
}