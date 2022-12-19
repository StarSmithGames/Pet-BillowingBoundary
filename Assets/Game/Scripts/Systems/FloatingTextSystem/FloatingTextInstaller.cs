using Game.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Systems.FloatingTextSystem
{
	[CreateAssetMenu(fileName = "FloatingTextInstaller", menuName = "Installers/FloatingTextInstaller")]
	public class FloatingTextInstaller : ScriptableObjectInstaller<FloatingTextInstaller>
	{
		public FloatingText floatingTextPrefab;

		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<FloatingTextSystem>().AsSingle();

			Container
				.BindFactory<FloatingText, FloatingText.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(3)
				.FromComponentInNewPrefab(floatingTextPrefab)
				.UnderTransform(x => x.Container.Resolve<UISubCanvas>().VFX))
				.WhenInjectedInto<FloatingTextSystem>();
		}
	}
}