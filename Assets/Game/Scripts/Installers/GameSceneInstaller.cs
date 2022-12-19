using Game.Entities;
using Game.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
	{
		[SerializeField] private UISubCanvas subCanvas;
		[SerializeField] private ClickerConveyor conveyor;

		public override void InstallBindings()
		{
			Container.BindInstance(subCanvas);
			Container.BindInstance(conveyor);

			Container.Bind<Player>().AsSingle().NonLazy();
		}
	}
}