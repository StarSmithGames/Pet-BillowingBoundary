using Game.Managers.AsyncManager;
using Game.Managers.GameManager;
using Game.Managers.NetworkTimeManager;
using Game.Managers.VibrationManager;
using Game.Systems.ApplicationHandler;
using Game.Systems.LocalizationSystem;

using Zenject;

namespace Game.Installers
{
	public class ProjectInstaller : MonoInstaller<ProjectInstaller>
	{
		public override void InstallBindings()
		{
			SignalBusInstaller.Install(Container);

			Container.Bind<AsyncManager>().FromNewComponentOnNewGameObject().AsSingle();

			ApplicationHandlerInstaller.Install(Container);
			NetworkTimeInstaller.Install(Container);
			GameManagerInstaller.Install(Container);
			LocalizationSystemInstaller.Install(Container);
			VibrationManagerInstaller.Install(Container);
		}
	}
}