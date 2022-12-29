using System.Collections.Generic;
using System.Linq;

using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Game.Entities
{
	public class Player
	{
		public event UnityAction onTapChanged;

		public PlayerSheet PlayerSheet { get; }

		public Taps Taps { get; }
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

		public Player()
		{
			PlayerSheet = new PlayerSheet();

			Gold = new(new BFN(1000000, 0).compressed);
			TapGold = new(BFN.Zero);
			TapGoldMultiplier = new(1f);
			TapGoldChance = new(0f);

			Taps = new(0);
			TapDamage = new(BFN.One);
			TapCriticalPower = new(1f);
			TapCriticalChance = new(0f);

			BonusRegistrator = new();
			SkillRegistrator = new();

			TargetsDefeat = new(0);
			BossesDefeat = new(0);


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
		}

		private void OnTapChanged()
		{
			onTapChanged?.Invoke();
		}

		public Data GetData()
		{
			return new Data()
			{
				gold = Gold.CurrentValue,

				tapsCount = Taps.CurrentValue,
				targetsDefeat = TargetsDefeat.CurrentValue,
				bossesDefeat = BossesDefeat.CurrentValue,

				bonuses = BonusRegistrator.GetData(),
				fireFistData = SkillRegistrator.GetFireFistData(),
			};
		}

		public class Data
		{
			public BFN gold;

			public int tapsCount;
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
			return (registers.Find((x) => x is FireFistSkill) as FireFistSkill).GetData();
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