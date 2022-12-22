using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Managers.NetworkTimeManager
{
	public class NetworkTimeManager : IInitializable
	{
		public void Initialize()
		{

		}

		public bool IsTrustedTime()
		{
			return true;
		}

		public DateTime GetDateTimeNow()
		{
			return GetTime.Now;
		}
	}
}