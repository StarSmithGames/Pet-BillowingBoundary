using Game.Managers.StorageManager;
using Game.Systems.AchievementSystem;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

using Zenject;

namespace Game.Entities
{
	public class Player
	{
		public event UnityAction onTapChanged;

		public PlayerSheet PlayerSheet { get; }

		public Taps Taps { get; }
		public Taps CriticalTaps { get; }
		public TapDamage TapDamage { get; }
		public TapCriticalPower TapCriticalPower { get; }
		public TapCriticalChance TapCriticalChance { get; }

		public Gold Gold { get; }
		public TapGold TapGold { get; }
		public TapGoldMultiplier TapGoldMultiplier { get; }
		public TapGoldChance TapGoldChance { get; }

		public BonusRegistrator BonusRegistrator { get; }
		public SkillRegistrator SkillRegistrator { get; }

		public Targets TargetsDefeat { get; }
		public Bosses BossesDefeat { get; }

		private BFN M = BFN.Million;
		private BFN B = BFN.Billion;
		private BFN T = BFN.Trillion;

		private AchievementSystem achievementSystem;

		public Player(ISaveLoad saveLoad, AchievementSystem achievementSystem)
		{
			this.achievementSystem = achievementSystem;

			PlayerSheet = new PlayerSheet();

			TapGold = new(BFN.Zero);
			TapGoldMultiplier = new(1f);
			TapGoldChance = new(0f);

			TapDamage = new(BFN.One);
			TapCriticalPower = new(1f);
			TapCriticalChance = new(0f);

			BonusRegistrator = new();
			SkillRegistrator = new();

			if (!saveLoad.GetStorage().IsFirstTime.GetData())
			{
				var data = saveLoad.GetStorage().Profile.GetData().playerData;

				Gold = new(data.gold.compressed);

				Taps = new(data.tapsCount);
				CriticalTaps = new(data.criticalTapsCount);

				TargetsDefeat = new(data.targetsDefeat);
				BossesDefeat = new(data.bossesDefeat);
			}
			else//first time
			{
				Gold = new(BFN.Zero);

				Taps = new(0);
				CriticalTaps = new(0);

				TargetsDefeat = new(0);
				BossesDefeat = new(0);
			}

			Gold.onChanged += OnGoldChanged;

			Taps.onChanged += OnTapsChanged;

			TapGold.onChanged += OnTapChanged;
			TapGold.onModifiersChanged += OnTapChanged;
			TapGoldMultiplier.onChanged += OnTapChanged;
			TapGoldMultiplier.onModifiersChanged += OnTapChanged;
			TapGoldChance.onChanged += OnTapChanged;
			TapGoldChance.onModifiersChanged += OnTapChanged;

			TapDamage.onChanged += OnTapChanged;
			TapDamage.onModifiersChanged += OnTapChanged;
			TapCriticalPower.onChanged += OnTapChanged;
			TapCriticalPower.onModifiersChanged += OnTapChanged;

			TargetsDefeat.onChanged += OnTargetsDefeated;
		}

		private void OnGoldChanged()
		{
			if (Gold.CurrentValue >= T)
			{
				achievementSystem.Achive_achievement_the_richest_man_on_the_planet();
			}
			else if(Gold.CurrentValue >= B)
			{
				achievementSystem.Achive_achievement_iam_a_rich_now();
			}
			else if(Gold.CurrentValue >= M)
			{
				achievementSystem.Achive_achievement_oh_wow_okay();
			}
		}

		private void OnTapsChanged()
		{
			if(Taps.CurrentValue == 100)
			{
				achievementSystem.Achive_achievement_100_times();
			}
			else if(Taps.CurrentValue == 1000)
			{
				achievementSystem.Achive_achievement_1000_times();
			}
			else if(Taps.CurrentValue == 10000)
			{
				achievementSystem.Achive_achievement_10000_times();
			}
		}

		private void OnTapChanged()
		{
			onTapChanged?.Invoke();
		}

		private void OnTargetsDefeated()
		{
			if(TargetsDefeat.CurrentValue == 100)
			{
				achievementSystem.Achive_achievement_overkill();
			}
		}

		public Data GetData()
		{
			return new Data()
			{
				gold = Gold.CurrentValue,

				tapsCount = Taps.CurrentValue,
				criticalTapsCount = CriticalTaps.CurrentValue,
				targetsDefeat = TargetsDefeat.CurrentValue,
				bossesDefeat = BossesDefeat.CurrentValue,

				bonuses = BonusRegistrator.GetData(),
				fireFistData = SkillRegistrator.GetFireFistData(),
			};
		}

		[System.Serializable]
		public class Data
		{
			public BFN gold;

			public int tapsCount;
			public int criticalTapsCount;
			public int targetsDefeat;
			public int bossesDefeat;

			public List<Bonus.Data> bonuses = new List<Bonus.Data>();
			public FireFistSkill.Data fireFistData;
		}
	}

	

	public class BonusRegistrator : Registrator<Bonus>
	{
		public List<Bonus.Data> GetData()
		{
			return new List<Bonus.Data>(registers.Select((x) => x.GetData()));
		}
	}

	public class SkillRegistrator : Registrator<ActiveSkill>
	{
		public event UnityAction<ActiveSkill> onSelectedSkillChanged;

		public ActiveSkill CurrentSkill { get; private set; }

		public void SelectSkill(ActiveSkill skill)
		{
			Assert.IsTrue(registers.Contains(skill));

			CurrentSkill = skill;

			onSelectedSkillChanged?.Invoke(CurrentSkill);
		}

		public FireFistSkill.Data GetFireFistData()
		{
			return GetAs<FireFistSkill>().GetData();
		}
	}

	public class Gold : AtributeBFN
	{
		public Gold(BFN currentValue) : base(currentValue) { }
	}

	#region Tap

	/// <summary>
	/// max value 2147483647
	/// </summary>
	public class Taps : Attribute<int>
	{
		public Taps(int currentValue) : base(currentValue) { }
	}
	/// <summary>
	/// max value 1E+9223372036854776115
	/// </summary>
	public class TapDamage : AttributeModifiableBFN
	{
		public TapDamage(BFN currentValue) : base(currentValue) { }
	}

	public class TapCriticalPower : AttributeModifiableFloat
	{
		public TapCriticalPower(float currentValue) : base(currentValue) { }
	}

	public class TapCriticalChance : AttributeModifiableFloat
	{
		public TapCriticalChance(float currentValue) : base(currentValue) { }
	}

	public class TapGold : AttributeModifiableBFN
	{
		public TapGold(BFN currentValue) : base(currentValue) { }
	}

	public class TapGoldMultiplier : AttributeModifiableFloat
	{
		public TapGoldMultiplier(float currentValue) : base(currentValue) { }
	}

	public class TapGoldChance : AttributeModifiableFloat
	{
		public TapGoldChance(float currentValue) : base(currentValue) { }
	}
	#endregion

	public class Targets : Attribute<int>
	{
		public Targets(int currentValue) : base(currentValue) { }
	}

	public class Bosses : Attribute<int>
	{
		public Bosses(int currentValue) : base(currentValue) { }
	}
}