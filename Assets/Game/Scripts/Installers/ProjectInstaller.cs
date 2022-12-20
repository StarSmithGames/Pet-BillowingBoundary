using Game.Managers.GameManager;

using Zenject;

namespace Game.Installers
{
	public class ProjectInstaller : MonoInstaller<ProjectInstaller>
	{
		public override void InstallBindings()
		{
			SignalBusInstaller.Install(Container);

			GameManagerInstaller.Install(Container);
		}
	}
}