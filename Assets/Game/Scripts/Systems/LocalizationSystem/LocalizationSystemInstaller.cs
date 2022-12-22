using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Systems.LocalizationSystem
{
	public class LocalizationSystemInstaller : Installer<LocalizationSystemInstaller>
	{
		public override void InstallBindings()
		{
			Container.DeclareSignal<SignalLocalizationChanged>();

			Container.BindInterfacesAndSelfTo<LocalizationSystem>().AsSingle().NonLazy();
		}
	}

	public struct SignalLocalizationChanged { }
}