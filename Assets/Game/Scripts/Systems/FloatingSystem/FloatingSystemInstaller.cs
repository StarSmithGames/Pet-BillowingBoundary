using Game.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Systems.FloatingSystem
{
	[CreateAssetMenu(fileName = "FloatingSystemInstaller", menuName = "Installers/FloatingSystemInstaller")]
	public class FloatingSystemInstaller : ScriptableObjectInstaller<FloatingSystemInstaller>
	{
		public FloatingText floatingTextPrefab;
		public FloatingCoin3D floatingCoinPrefab;

		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<FloatingSystem>().AsSingle();

			Container
				.BindFactory<FloatingText, FloatingText.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(3)
				.FromComponentInNewPrefab(floatingTextPrefab)
				.UnderTransform(x => x.Container.Resolve<UISubCanvas>().VFX))
				.WhenInjectedInto<FloatingSystem>();

			Container
				.BindFactory<FloatingCoin3D, FloatingCoin3D.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(3)
				.FromComponentInNewPrefab(floatingCoinPrefab))
				.WhenInjectedInto<FloatingSystem>();
		}
	}
}