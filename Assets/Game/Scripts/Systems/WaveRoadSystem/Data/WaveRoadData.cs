using Game.Managers.ClickManager;

using Sirenix.OdinInspector;

using System.Collections.Generic;

using UnityEngine;

namespace Game.Systems.WaveRoadSystem
{
	[CreateAssetMenu(fileName = "WaveRoadData", menuName = "Game/Wave Road")]
	public class WaveRoadData : ScriptableObject
	{
		public MiddleRoad middleRoad;
		public EndRoad endRoad;
	}

	[System.Serializable]
	public class MiddleRoad
	{
		[Min(1)]
		public int countTargets = 1;
		public bool shuffle = true;
		public List<EnemyData> targets = new();
	}

	[System.Serializable]
	public class EndRoad
	{
		public bool isHasBoss = true;
		[ShowIf("isHasBoss")]
		public bool isRandomBoss = true;
		[ShowIf("@isHasBoss && !isRandomBoss")]
		public EnemyData boss;
		[ShowIf("@isHasBoss && isRandomBoss")]
		public List<EnemyData> bosses = new();
	}
}