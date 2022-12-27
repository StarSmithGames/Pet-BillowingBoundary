using Zenject;

namespace Game.Systems.ApplicationHandler
{
	public class ApplicationHandlerInstaller : Installer<ApplicationHandlerInstaller>
	{
		public override void InstallBindings()
		{
			Container.DeclareSignal<SignalApplicationQuit>();
			Container.DeclareSignal<SignalApplicationPause>();
			Container.DeclareSignal<SignalApplicationFocus>();

			Container.Bind<ApplicationHandler>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
		}
	}

	public struct SignalApplicationQuit { }
	public struct SignalApplicationPause { public bool trigger; }
	public struct SignalApplicationFocus { public bool trigger; }
}