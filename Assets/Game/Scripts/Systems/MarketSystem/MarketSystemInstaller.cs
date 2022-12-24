using Game.UI;
using UnityEngine;
using Zenject;

namespace Game.Systems.MarketSystem
{
	[CreateAssetMenu(fileName = "MarketSystemInstaller", menuName = "Installers/MarketSystemInstaller")]
	public class MarketSystemInstaller : ScriptableObjectInstaller<MarketSystemInstaller>
	{
		public UIMarketItem marketPrefab;

		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<MarketHandler>().AsSingle().NonLazy();

			Container.BindFactory<UIMarketItem, UIMarketItem.Factory>();
			Container
				.BindFactory<UIMarketItem, UIMarketItem.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(1)
				.FromComponentInNewPrefab(marketPrefab)
				.UnderTransform(x => x.Container.Resolve<UISubCanvas>().Windows))
				.WhenInjectedInto<MarketWindow>();
		}
	}
}