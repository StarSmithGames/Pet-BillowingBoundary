using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Game.Managers.AudioManager
{
	[CreateAssetMenu(fileName = "AudioManagerInstaller", menuName = "Installers/AudioManagerInstaller")]
	public class AudioManagerInstaller : ScriptableObjectInstaller<AudioManagerInstaller>
	{
		public AudioSettings settings;
		
		public override void InstallBindings()
		{
			Container.DeclareSignal<SignalMusicChanged>();

			Container.BindInstance(settings).WhenInjectedInto<AudioManager>();
			Container.BindFactory<AudioSource, AudioSource.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(2)
				.FromNewComponentOnNewGameObject())
				.WhenInjectedInto<AudioManager>();
			Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle().NonLazy();
		}
	}

	public struct SignalMusicChanged { }

	[System.Serializable]
	public class AudioSettings
	{
		[Header("Sounds")]
		public AudioClip buttonClick;
		public AudioClip bossDefeated;
		public AudioClip targetDefeated;
		public AudioClip coinsReward;
		[Space]
		public List<AudioClip> hits = new List<AudioClip>();
		public AudioClip goldLamaHit;

		[Header("Music")]
		public AudioClip background;
	}
}