using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Entities
{
	public abstract class SkillData : ScriptableObject
	{
	}

	public abstract class PassiveSkillData : SkillData
	{
	}

	public abstract class ActiveSkillData : SkillData
	{
		public SkillLimitations limitations;
	}

	[System.Serializable]
	public class SkillLimitations
	{
		public bool isHasCooldown = true;
		[ShowIf("isHasCooldown")]
		[Min(1)]
		public float baseCooldown;
	}

	public class Cooldown
	{
		public event UnityAction<float, float> onChanged;

		public float Total { get; set; }

		public float Remaining
		{
			get => remaining;
			set
			{
				remaining = value;
				onChanged?.Invoke(remaining, Normalized);
			}
		}
		protected float remaining;

		public float Normalized => Remaining / Total;

		public void Tick()
		{
			Remaining -= Time.deltaTime;
		}

		public void Reset()
		{
			Remaining = Total;
		}
	}
}