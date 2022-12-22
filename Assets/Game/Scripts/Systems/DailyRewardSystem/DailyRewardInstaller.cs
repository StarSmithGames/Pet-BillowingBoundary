using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Systems.DailyRewardSystem
{
	[CreateAssetMenu(fileName = "DailyRewardInstaller", menuName = "Installers/DailyRewardInstaller")]
	public class DailyRewardInstaller : ScriptableObjectInstaller<DailyRewardInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<DailyRewardSystem>().AsSingle().NonLazy();
		}
	}
}