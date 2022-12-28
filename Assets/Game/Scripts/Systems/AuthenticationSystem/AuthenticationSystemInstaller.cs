using UnityEngine;

using Zenject;

namespace Game.Systems.AuthenticationSystem
{
    [CreateAssetMenu(fileName = "AuthenticationSystemInstaller", menuName = "Installers/AuthenticationSystemInstaller")]
    public class AuthenticationSystemInstaller : ScriptableObjectInstaller<AuthenticationSystemInstaller>
    {
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<FirebaseAuthentication>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<AuthenticationSystem>().AsSingle().NonLazy();
		}
	}
}