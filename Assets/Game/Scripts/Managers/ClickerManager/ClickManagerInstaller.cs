using UnityEngine;
using Zenject;

namespace Game.Managers.ClickManager
{
	[CreateAssetMenu(fileName = "ClickManagerInstaller", menuName = "Installers/ClickManagerInstaller")]
	public class ClickManagerInstaller : ScriptableObjectInstaller<ClickManagerInstaller>
	{
		public TargetSettings targetSettings;

		public override void InstallBindings()
		{
			Container.DeclareSignal<SignalTouchChanged>();
			Container.DeclareSignal<SignalTargetChanged>();

			Container.BindInterfacesAndSelfTo<ClickHandler>().AsSingle().NonLazy();

			Container.BindInstance(targetSettings).WhenInjectedInto<TargetHandler>();
			Container.BindInterfacesAndSelfTo<TargetHandler>().AsSingle().NonLazy();
		}
	}

	public struct SignalTouchChanged
	{
		public Touch touch;
	}

	public struct SignalTargetChanged
	{
		public ClickableObject target;
	}
}