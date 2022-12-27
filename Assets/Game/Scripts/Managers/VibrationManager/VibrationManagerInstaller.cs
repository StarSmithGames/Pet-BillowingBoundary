using Zenject;

namespace Game.Managers.VibrationManager
{
	public class VibrationManagerInstaller : Installer<VibrationManagerInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<VibrationManager>().AsSingle().NonLazy();
		}
	}
}