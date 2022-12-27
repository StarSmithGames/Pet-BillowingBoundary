using UnityEngine;

using Zenject;

namespace Game.Systems.AnalyticsSystem
{
	[CreateAssetMenu(fileName = "AnalyticsSystemInstaller", menuName = "Installers/AnalyticsSystemInstaller")]
	public class AnalyticsSystemInstaller : ScriptableObjectInstaller<AnalyticsSystemInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<AnalyticsSystem>().AsSingle().NonLazy();
		}
	}
}