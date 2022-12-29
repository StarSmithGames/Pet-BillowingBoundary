using Game.Managers.ClickManager;

using Sirenix.OdinInspector;

using UnityEngine;

namespace Game.Entities
{
	[System.Serializable]
	[CreateAssetMenu(fileName = "TargetData", menuName = "Game/TargetData")]
	public class TargetData : ScriptableObject
	{
		public Information information;

		public int baseHealthPoints;

		[Space]
		#region Coins After Defeat
		[Header("Coins After Defeat")]
		public bool isHasCoinsAfterDefeat = true;
		[ShowIf("@isHasCoinsAfterDefeat && !isCoinsRandomAfterDefeat")]
		public int baseCoinsAfterDefeat;
		[ShowIf("isHasCoinsAfterDefeat")]
		public bool isCoinsRandomAfterDefeat = false;
		[ShowIf("@isHasCoinsAfterDefeat && isCoinsRandomAfterDefeat")]
		public int baseMinCoinsAfterDefeat;
		[ShowIf("@isHasCoinsAfterDefeat && isCoinsRandomAfterDefeat")]
		public int baseMaxCoinsAfterDefeat;
		#endregion

		#region Coins On Punch
		[Header("Coins On Punch")]
		public bool isHasCoinsOnPunch = true;
		[ShowIf("isHasCoinsOnPunch")]
		public bool isPlayerChance = true;
		[ShowIf("@isHasCoinsOnPunch && !isCoinsRandomOnPunch")]
		public int baseCoinsOnPunch;
		[ShowIf("isHasCoinsOnPunch")]
		public bool isCoinsRandomOnPunch = false;
		[ShowIf("@isHasCoinsOnPunch && isCoinsRandomOnPunch")]
		public int baseMinCoinsOnPunch;
		[ShowIf("@isHasCoinsOnPunch && isCoinsRandomOnPunch")]
		public int baseMaxCoinsOnPunch;
		[Space]
		[ShowIf("isHasCoinsOnPunch")]
		public CoinsOnPunchType coinsOnPunchType = CoinsOnPunchType.Mod;
		[ShowIf("@isHasCoinsOnPunch && coinsOnPunchType == CoinsOnPunchType.Mod")]
		[Min(1)]
		public int mod = 2;
		#endregion

		[Space]
		public PunchData smallPunch;
		[Space]
		public ClickableObject prefab;
		public Vector3 initPosition;
		public Quaternion initRotation;

		public int GetCoinsAfterDefeat()
		{
			if (isHasCoinsAfterDefeat)
			{
				if (isCoinsRandomAfterDefeat)
				{
					return Random.Range(baseMinCoinsAfterDefeat, baseMaxCoinsAfterDefeat);
				}
				return baseCoinsAfterDefeat;
			}
			return 0;
		}

		public int GetCoinsOnPunch()
		{
			if (isHasCoinsOnPunch)
			{
				if (isCoinsRandomOnPunch)
				{
					return Random.Range(baseMinCoinsOnPunch, baseMaxCoinsOnPunch);
				}
				return baseCoinsOnPunch;
			}
			return 0;
		}
	}
	public enum CoinsOnPunchType
	{
		Mod
	}
}