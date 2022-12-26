using Game.Systems.LocalizationSystem;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Zenject;

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
		public UnityAction<ActiveSkill> onChanged;
		public abstract SkillData Data { get; }

		public Cooldown Cooldown { get; private set; }
		protected bool isHasCooldown = true;
		protected bool isCooldown = false;

		protected Player player;
		protected LocalizationSystem localizationSystem;

		[Inject]
		private void Construct(Player player, LocalizationSystem localizationSystem)
		{
			this.player = player;
			this.localizationSystem = localizationSystem;
		}

		protected virtual void Start()
		{
			player.SkillRegistrator.Registrate(this);

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

		public abstract void PurchaseProperty(int index);
		public abstract SkillProperty GetProperty(int index);

		protected virtual void ResetSkill()
		{
			isCooldown = false;
		}
	}
}