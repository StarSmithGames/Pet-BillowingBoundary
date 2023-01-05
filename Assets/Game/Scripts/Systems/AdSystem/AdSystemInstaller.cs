using UnityEngine;

using Zenject;

namespace Game.Systems.AdSystem
{
	[CreateAssetMenu(fileName = "AdSystemInstaller", menuName = "Installers/AdSystemInstaller")]
	public class AdSystemInstaller : ScriptableObjectInstaller<AdSystemInstaller>
	{
		public string appId;
		public bool isDebug = false;

		public override void InstallBindings()
		{
			Container.DeclareSignal<SignalADSEnableChanged>();

			Container.BindInstance(appId).WhenInjectedInto<AdSystem>();
			Container.BindInstance(isDebug).WhenInjectedInto<AdSystem>();
			Container.BindInterfacesAndSelfTo<AdSystem>().AsSingle().NonLazy();

			Container.BindInterfacesAndSelfTo<AdBanner>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<AdInterstitial>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<AdRewarded>().AsSingle().NonLazy();

			Container.BindInterfacesTo<AdSystemHandler>().AsSingle().NonLazy();
		}
	}

	public struct SignalADSEnableChanged
	{
		public bool trigger;
	}
}