using Game.Managers.GameManager;
using Game.Managers.NetworkTimeManager;
using Game.Systems.ApplicationHandler;

using Zenject;

namespace Game.Installers
{
	public class ProjectInstaller : MonoInstaller<ProjectInstaller>
	{
		public override void InstallBindings()
		{
			SignalBusInstaller.Install(Container);

			ApplicationHandlerInstaller.Install(Container);
			NetworkTimeInstaller.Install(Container);
			GameManagerInstaller.Install(Container);
		}
	}
}