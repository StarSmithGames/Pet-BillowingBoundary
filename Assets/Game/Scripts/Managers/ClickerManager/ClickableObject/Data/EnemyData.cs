﻿using Sirenix.OdinInspector;

using UnityEngine;

namespace Game.Managers.ClickManager
{
	[System.Serializable]
	[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy")]
	public class EnemyData : ScriptableObject
	{
		public Information information;

		public BFN baseHealthPoints;
		[Space]
		#region Coins After Defeat
		[Header("Coins After Defeat")]
		public bool isHasCoinsAfterDefeat = true;
		[ShowIf("@isHasCoinsAfterDefeat && !isCoinsRandomAfterDefeat")]
		public BFN baseCoinsAfterDefeat;
		[ShowIf("isHasCoinsAfterDefeat")]
		public bool isCoinsRandomAfterDefeat = false;
		[ShowIf("@isHasCoinsAfterDefeat && isCoinsRandomAfterDefeat")]
		public BFN baseMinCoinsAfterDefeat;
		[ShowIf("@isHasCoinsAfterDefeat && isCoinsRandomAfterDefeat")]
		public BFN baseMaxCoinsAfterDefeat;
		#endregion

		#region Coins On Punch
		[Header("Coins On Punch")]
		public bool isHasCoinsOnPunch = true;
		[ShowIf("@isHasCoinsOnPunch && !isCoinsRandomOnPunch")]
		public BFN baseCoinsOnPunch;
		[ShowIf("isHasCoinsOnPunch")]
		public bool isCoinsRandomOnPunch = false;
		[ShowIf("@isHasCoinsOnPunch && isCoinsRandomOnPunch")]
		public BFN baseMinCoinsOnPunch;
		[ShowIf("@isHasCoinsOnPunch && isCoinsRandomOnPunch")]
		public BFN baseMaxCoinsOnPunch;
		[Space]
		[ShowIf("isHasCoinsOnPunch")]
		public CoinsOnPunchType coinsOnPunchType = CoinsOnPunchType.Mod;
		[ShowIf("@isHasCoinsOnPunch && coinsOnPunchType == CoinsOnPunchType.Mod")]
		[Min(1)]
		public int mod = 2;
		#endregion

		[Space]
		public ClickableObject prefab;

		public BFN GetCoinsAfterDefeat()
		{
			if (isHasCoinsAfterDefeat)
			{
				if (isCoinsRandomAfterDefeat)
				{
					return BFN.RandomRange(baseMinCoinsAfterDefeat, baseMaxCoinsAfterDefeat);
				}
				return baseCoinsAfterDefeat;
			}
			return BFN.Zero;
		}

		public BFN GetCoinsOnPunch()
		{
			if (isHasCoinsOnPunch)
			{
				if (isCoinsRandomOnPunch)
				{
					return BFN.RandomRange(baseMinCoinsOnPunch, baseMaxCoinsOnPunch);
				}
				return baseCoinsOnPunch;
			}
			return BFN.Zero;
		}
	}
	public enum CoinsOnPunchType
	{
		Mod
	}
}