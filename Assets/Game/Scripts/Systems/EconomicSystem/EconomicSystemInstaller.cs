using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Systems.EconomicSystem
{
	[CreateAssetMenu(fileName = "EconomicSystemInstaller", menuName = "Installers/EconomicSystemInstaller")]
	public class EconomicSystemInstaller : ScriptableObjectInstaller<EconomicSystemInstaller>
	{
		public Economics economics;

		public override void InstallBindings()
		{
			Container.BindInstance(economics).WhenInjectedInto<EconomicSystem>();
			Container.BindInterfacesAndSelfTo<EconomicSystem>().AsSingle().NonLazy();
		}
	}

	[System.Serializable]
	public class Economic
	{

	}

	[System.Serializable]
	public class Economics
	{
		public Economic common;
	}
}