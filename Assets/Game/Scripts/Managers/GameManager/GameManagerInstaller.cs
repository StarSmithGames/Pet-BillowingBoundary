using Zenject;

namespace Game.Managers.GameManager
{
	public class GameManagerInstaller : Installer<GameManagerInstaller>
	{
		public override void InstallBindings()
		{
			Container.DeclareSignal<SignalGameStateChanged>();

			Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
		}
	}

	public struct SignalGameStateChanged
	{
		public GameState newGameState;
		public GameState oldGameState;
	}
}