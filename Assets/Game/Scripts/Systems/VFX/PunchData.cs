using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PunchData", menuName = "Game/Effects/Punch")]
public class PunchData : ScriptableObject
{
	[HideLabel]
	public PunchSettings settings;
}

[System.Serializable]
public class PunchSettings
{
	public Punch punch;
	public float duration = 0.25f;
	public int vibrato = 10;
	public float elasticity = 1f;

	public Vector3 GetPunch()
	{
		if (punch.isPunchRandom)
		{
			if (punch.stretchingAxis)
			{
				int axis = Random.Range(0, 2);
				if(axis == 0)
				{
					return new Vector3(punch.randomXLimits.RandomBtw(), 0, 0);
				}
				else if (axis == 1)
				{
					return new Vector3(0, punch.randomYLimits.RandomBtw(), 0);
				}
				else if(axis == 2)
				{
					return new Vector3(0, 0, punch.randomZLimits.RandomBtw());
				}
			}
			return new Vector3(punch.randomXLimits.RandomBtw(), punch.randomYLimits.RandomBtw(), punch.randomZLimits.RandomBtw());
		}

		return punch.punch;
	}

	[System.Serializable]
	public class Punch
	{
		public bool isPunchRandom = false;
		[ShowIf("isPunchRandom")]
		public bool stretchingAxis = false;

		[HideIf("isPunchRandom")]
		public Vector3 punch = Vector3.one;
		[ShowIf("isPunchRandom")]
		public Vector2 randomXLimits;
		[ShowIf("isPunchRandom")]
		public Vector2 randomYLimits;
		[ShowIf("isPunchRandom")]
		public Vector2 randomZLimits;
	}
}
