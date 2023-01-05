using UnityEngine;

using Zenject;

namespace Game.Systems.AnalyticsSystem
{
	[CreateAssetMenu(fileName = "AnalyticsSystemInstaller", menuName = "Installers/AnalyticsSystemInstaller")]
	public class AnalyticsSystemInstaller : ScriptableObjectInstaller<AnalyticsSystemInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<AmplitudeAnalyticsGroup>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<FirebaseAnalyticsGroup>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<UnityAnalyticsGroup>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<AnalyticsSystem>().AsSingle().NonLazy();
		}
	}
}