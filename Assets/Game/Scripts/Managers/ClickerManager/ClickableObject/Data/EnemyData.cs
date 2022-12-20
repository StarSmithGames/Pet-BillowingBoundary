using UnityEngine;

namespace Game.Managers.ClickManager
{
	[System.Serializable]
	[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy")]
	public class EnemyData : ScriptableObject
	{
		[Min(1)]
		public int tapCount;
		public ClickableObject prefab;
	}
}