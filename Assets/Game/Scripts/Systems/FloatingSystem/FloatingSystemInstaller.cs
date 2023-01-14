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
		public FloatingTextUI floatingText2DPrefab;
		public FloatingCoin2D floatingCoin2DPrefab;
		public FloatingCoin3D floatingCoin3DPrefab;
		public FloatingCandy3D floatingCandy3DPrefab;

		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<FloatingSystem>().AsSingle();

			Container.BindInterfacesAndSelfTo<FloatingAwards>().AsSingle();

			Container
				.BindFactory<FloatingText, FloatingText.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(1)
				.FromComponentInNewPrefab(floatingTextPrefab)
				.UnderTransform(x => x.Container.Resolve<UISubCanvas>().VFX))
				.WhenInjectedInto<FloatingSystem>();

			Container
				.BindFactory<FloatingTextUI, FloatingTextUI.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(12).WithMaxSize(12)
				.FromComponentInNewPrefab(floatingText2DPrefab)
				.UnderTransform(x => x.Container.Resolve<UISubCanvas>().VFX))
				.WhenInjectedInto<FloatingSystem>();

			Container
				.BindFactory<FloatingCoin2D, FloatingCoin2D.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(100).WithMaxSize(200)
				.FromComponentInNewPrefab(floatingCoin2DPrefab)
				.UnderTransform(x => x.Container.Resolve<UISubCanvas>().VFX))
				.WhenInjectedInto<FloatingSystem>();

			Container
				.BindFactory<FloatingCoin3D, FloatingCoin3D.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(25).WithMaxSize(100)
				.FromComponentInNewPrefab(floatingCoin3DPrefab))
				.WhenInjectedInto<FloatingSystem>();

			Container
				.BindFactory<FloatingCandy3D, FloatingCandy3D.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(25).WithMaxSize(100)
				.FromComponentInNewPrefab(floatingCandy3DPrefab))
				.WhenInjectedInto<FloatingSystem>();
		}
	}
}