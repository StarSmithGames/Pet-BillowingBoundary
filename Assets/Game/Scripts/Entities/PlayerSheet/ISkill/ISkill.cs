using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
    public interface ISkill
    {
		SkillData Data { get; }
	}

    public abstract class PassiveSkill : ISkill
    {
		public SkillData Data => data;
		private PassiveSkillData data;
	}
    public abstract class ActiveSkill : MonoBehaviour, ISkill
	{
		public abstract SkillData Data { get; }

		public Cooldown Cooldown { get; private set; }
		protected bool isHasCooldown = true;
		protected bool isCooldown = false;

		protected virtual void Start()
		{
			isHasCooldown = (Data as ActiveSkillData).limitations.isHasCooldown;
			if (isHasCooldown)
			{
				Cooldown = new Cooldown();
			}

			ResetSkill();
		}

		protected virtual void Update()
		{
			if (isHasCooldown)
			{
				if (isCooldown)
				{
					Cooldown.Tick();
					if (Cooldown.Remaining <= 0)
					{
						ResetSkill();
						isCooldown = false;
					}
				}
			}
		}

		protected virtual void ResetSkill()
		{
			isCooldown = false;
		}
	}
}