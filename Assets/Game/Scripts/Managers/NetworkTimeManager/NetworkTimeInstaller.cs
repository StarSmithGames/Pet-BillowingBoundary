using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Managers.NetworkTimeManager
{
	public class NetworkTimeInstaller : Installer<NetworkTimeInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<NetworkTimeManager>().AsSingle().NonLazy();
		}
	}
}