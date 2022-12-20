using Game.Entities;
using Game.Managers.ClickManager;
using Game.Systems.CameraSystem;
using Game.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
	{
		[SerializeField] private CameraSystem cameraSystem;
		[SerializeField] private UISubCanvas subCanvas;
		[SerializeField] private ClickerConveyor conveyor;

		public override void InstallBindings()
		{
			Container.BindInstance(cameraSystem);
			Container.BindInstance(subCanvas);
			Container.BindInstance(conveyor);

			Container.Bind<Player>().AsSingle().NonLazy();
		}
	}
}