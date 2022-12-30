using Game.Managers.StorageManager;
using UnityEngine;

using Zenject;

namespace Game.Managers.AudioManager
{
    public class AudioManager : IInitializable
    {
		private AudioSource background;

		private SignalBus signalBus;
		private AudioSettings settings;
		private AudioSource.Factory audioFactory;
		private ISaveLoad saveLoad;

		public AudioManager(SignalBus signalBus, AudioSettings settings, AudioSource.Factory audioFactory, ISaveLoad saveLoad)
        {
			this.signalBus = signalBus;
			this.settings = settings;
			this.audioFactory = audioFactory;
			this.saveLoad = saveLoad;
		}

		public void Initialize()
		{
			signalBus?.Subscribe<SignalMusicChanged>(OnMusicChanged);

			PlayBackground();
		}

		public void PlayBackground()
		{
			background = audioFactory.Create();
			background.Mute(!saveLoad.GetStorage().IsMusic.GetData());
			background.PlayLoop(settings.background);
		}

		public void PlayButtonClick()
		{
			PlaySoundOnce(settings.buttonClick);
		}

		public void PlayBossDefeated()
		{
			PlaySoundOnce(settings.bossDefeated);
		}

		public void PlayCoinsReward()
		{
			PlaySoundOnce(settings.coinsReward);
		}

		private void PlaySoundOnce(AudioClip clip)
		{
			if (!saveLoad.GetStorage().IsSound.GetData()) return;

			var source = audioFactory.Create();
			source.PlayOnce(clip);
		}

		private void OnMusicChanged()
		{
			if(background != null)
			{
				background.Mute(!saveLoad.GetStorage().IsMusic.GetData());
			}
		}
	}
}