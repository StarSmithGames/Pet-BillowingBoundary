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
		[Min(0)]
		public float releaseDuration = 5f;
		[Space]
		[Min(0)]
		public float waitingTime = 5f;
		[Min(0)]
		public float decreaseSpeed = 5f;
		public AnimationCurve decreaseCurve;
	}
}