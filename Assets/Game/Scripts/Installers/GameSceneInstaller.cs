using Game.Entities;
using Game.Managers.ClickManager;
using Game.Systems.CameraSystem;
using Game.Systems.WaveRoadSystem;
using Game.UI;

using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
	{
		[Header("Data")]
		[SerializeField] private WaveRoadPatternData pattern;
		[Header("Components")]
		[SerializeField] private CameraSystem cameraSystem;
		[SerializeField] private UISubCanvas subCanvas;
		[SerializeField] private FastMessageWindow fastMessageWindowPrefab;
		[SerializeField] private UIPieceAnimatedBar pieceAnimatedBarPrefab;
		[Header("Out")]
		[SerializeField] private ClickStarter conveyor;

		public override void InstallBindings()
		{
			Container.BindInstance(cameraSystem);
			Container.BindInstance(subCanvas);
			Container.BindInstance(conveyor);

			Container.Bind<Player>().AsSingle().NonLazy();
			Container.BindInstance(pattern).WhenInjectedInto<WaveRoad>();
			Container.Bind<WaveRoad>().AsSingle().NonLazy();

			//FastMessageWindow 1
			Container.BindFactory<FastMessageWindow, FastMessageWindow.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(1)
				.FromComponentInNewPrefab(fastMessageWindowPrefab)
				.UnderTransform((x) => x.Container.Resolve<UISubCanvas>().Windows));

			//UIPieceAnimatedBar 5
			Container.BindFactory<UIPieceAnimatedBar, UIPieceAnimatedBar.Factory>()
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(5)
				.FromComponentInNewPrefab(pieceAnimatedBarPrefab)
				.UnderTransform((x) => x.Container.Resolve<UISubCanvas>().Windows));
		}
	}
}