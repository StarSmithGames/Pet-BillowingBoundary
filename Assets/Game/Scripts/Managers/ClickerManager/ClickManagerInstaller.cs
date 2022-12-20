using UnityEngine;
using Zenject;

namespace Game.Managers.ClickManager
{
	[CreateAssetMenu(fileName = "ClickManagerInstaller", menuName = "Installers/ClickManagerInstaller")]
	public class ClickManagerInstaller : ScriptableObjectInstaller<ClickManagerInstaller>
	{
		public override void InstallBindings()
		{
			Container.DeclareSignal<SignalTouchChanged>();
			Container.DeclareSignal<SignalTargetChanged>();

			Container.BindInterfacesAndSelfTo<ClickHandler>().AsSingle().NonLazy();
		}
	}

	public struct SignalTouchChanged
	{
		public Touch touch;
	}

	public struct SignalTargetChanged
	{
		public ClickableObject clickable;
	}
}