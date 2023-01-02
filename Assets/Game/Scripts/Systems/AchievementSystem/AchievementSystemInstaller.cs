using UnityEngine;

using Zenject;

namespace Game.Systems.AchievementSystem
{
	[CreateAssetMenu(fileName = "AchievementSystemInstaller", menuName = "Installers/AchievementSystemInstaller")]
	public class AchievementSystemInstaller : ScriptableObjectInstaller<AchievementSystemInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<GooglePlayAchievements>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<AchievementSystem>().AsSingle().NonLazy();
		}
	}
}