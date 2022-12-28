using Game.UI;
using UnityEngine;
using Zenject;

namespace Game.Systems.MarketSystem
{
	[CreateAssetMenu(fileName = "MarketSystemInstaller", menuName = "Installers/MarketSystemInstaller")]
	public class MarketSystemInstaller : ScriptableObjectInstaller<MarketSystemInstaller>
	{
		public UIMarketBonusItem marketBonusPrefab;
		public UIMarketSkillItem marketSkillPrefab;

		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<MarketHandler>().AsSingle().NonLazy();

			Container
				.BindFactory<UIMarketBonusItem, UIMarketBonusItem.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(5)
				.FromComponentInNewPrefab(marketBonusPrefab)
				.UnderTransform(x => x.Container.Resolve<UISubCanvas>().Windows))
				.WhenInjectedInto<MarketWindow>();

			Container
				.BindFactory<UIMarketSkillItem, UIMarketSkillItem.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(2)
				.FromComponentInNewPrefab(marketSkillPrefab)
				.UnderTransform(x => x.Container.Resolve<UISubCanvas>().Windows))
				.WhenInjectedInto<MarketWindow>();
		}
	}
}