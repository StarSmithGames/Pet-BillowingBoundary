using UnityEngine;

using Zenject;

namespace Game.Managers.IAPManager
{
	[CreateAssetMenu(fileName = "IAPManagerInstaller", menuName = "Installers/IAPManagerInstaller")]
	public class IAPManagerInstaller : ScriptableObjectInstaller<IAPManagerInstaller>
	{
		public override void InstallBindings()
		{
			Container.Bind<IAPManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
		}
	}
}