using UnityEngine;

using Zenject;

namespace Game.Managers.AudioManager
{
	[CreateAssetMenu(fileName = "AudioManagerInstaller", menuName = "Installers/AudioManagerInstaller")]
	public class AudioManagerInstaller : ScriptableObjectInstaller<AudioManagerInstaller>
	{
		public AudioSettings settings;
		public AudioSource audioSourcePrefab;
		
		public override void InstallBindings()
		{
			Container.BindInstance(settings).WhenInjectedInto<AudioManager>();
			Container.BindFactory<AudioSource, AudioSource.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(2)
				.FromComponentInNewPrefab(audioSourcePrefab))
				.WhenInjectedInto<AudioManager>();
			Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle().NonLazy();
		}
	}

	[System.Serializable]
	public class AudioSettings
	{
		[Header("Sounds")]
		public AudioClip buttonClick;
		public AudioClip bossDefeated;
		public AudioClip targetDefeated;
		public AudioClip coinsReward;
		[Header("Music")]
		public AudioClip background;
	}
}