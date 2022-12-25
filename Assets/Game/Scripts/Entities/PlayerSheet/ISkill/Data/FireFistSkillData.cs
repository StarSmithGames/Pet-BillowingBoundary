using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
	[CreateAssetMenu(fileName = "FireFistSkillData", menuName = "Game/Skills/FireFistSkill")]
	public class FireFistSkillData : ActiveSkillData
	{
		[Min(0)]
		public float incrementForTap = 1f;
		public float releaseDuration = 5f;
	}
}