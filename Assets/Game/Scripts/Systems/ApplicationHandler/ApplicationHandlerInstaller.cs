using Zenject;

namespace Game.Systems.ApplicationHandler
{
	public class ApplicationHandlerInstaller : Installer<ApplicationHandlerInstaller>
	{
		public override void InstallBindings()
		{
			Container.DeclareSignal<SignalApplicationQuit>();

			Container.Bind<ApplicationHandler>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
		}
	}

	public struct SignalApplicationQuit { }
}