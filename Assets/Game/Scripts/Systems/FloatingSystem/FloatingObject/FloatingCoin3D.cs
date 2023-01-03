using Zenject;

namespace Game.Systems.FloatingSystem
{
	public class FloatingCoin3D : Floating3D
	{
		public class Factory : PlaceholderFactory<FloatingCoin3D> { }
	}
}