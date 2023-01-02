using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Systems.DailyRewardSystem
{
	[CreateAssetMenu(fileName = "DailyRewardInstaller", menuName = "Installers/DailyRewardInstaller")]
	public class DailyRewardInstaller : ScriptableObjectInstaller<DailyRewardInstaller>
	{
		public DailyRewardSetting setting;

		public override void InstallBindings()
		{
			Container.BindInstance(setting).WhenInjectedInto<DailyRewardSystem>();
			Container.BindInterfacesAndSelfTo<DailyRewardSystem>().AsSingle().NonLazy();
		}
	}

	[System.Serializable]
	public class DailyRewardSetting
	{
		public List<DailyReward> rewards = new List<DailyReward>();
	}

	[System.Serializable]
	public class DailyReward
	{
		public DayType day;
		public BFN baseCoins;
	}
}