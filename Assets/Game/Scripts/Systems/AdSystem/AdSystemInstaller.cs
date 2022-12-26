using UnityEngine;

using Zenject;

namespace Game.Systems.AdSystem
{
	[CreateAssetMenu(fileName = "AdSystemInstaller", menuName = "Installers/AdSystemInstaller")]
	public class AdSystemInstaller : ScriptableObjectInstaller<AdSystemInstaller>
	{
		public string appId;

		public override void InstallBindings()
		{
			Container.BindInstance(appId).WhenInjectedInto<AdSystem>();
			Container.BindInterfacesAndSelfTo<AdSystem>().AsSingle().NonLazy();

			Container.BindInterfacesAndSelfTo<AdBanner>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<AdInterstitial>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<AdRewarded>().AsSingle().NonLazy();

			Container.BindInterfacesTo<AdSystemHandler>().AsSingle().NonLazy();
		}
	}
}