using UnityEngine;
using Zenject;

namespace Game.Managers.StorageManager
{
	[CreateAssetMenu(fileName = "StorageManagerInstaller", menuName = "Installers/StorageManagerInstaller")]
	public class StorageManagerInstaller : ScriptableObjectInstaller<StorageManagerInstaller>
	{
		public PlayerPrefsSaveLoad.Settings playerPrefsSettings;

		public override void InstallBindings()
		{
			Container.DeclareSignal<SignalSave>();

			Container.BindInstance(playerPrefsSettings).WhenInjectedInto<PlayerPrefsSaveLoad>();
			Container.BindInterfacesTo<PlayerPrefsSaveLoad>().AsSingle().NonLazy();
		}
	}

	public struct SignalSave { }
}