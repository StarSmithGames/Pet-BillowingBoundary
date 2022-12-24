using UnityEngine;
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

		public Player()
		{
			PlayerSheet = new PlayerSheet();

			Gold = new Gold(new BFN(1000, 0).compressed);
			TapGold = new TapGold(BFN.Zero);
			TapGoldMultiplier = new TapGoldMultiplier(1f);
			TapGoldChance = new TapGoldChance(0f);

			Taps = new Taps(0);
			TapDamage = new TapDamage(BFN.One);
			TapCriticalPower = new TapCriticalPower(1f);
			TapCriticalChance = new TapCriticalChance(0f);

			BonusRegistrator = new BonusRegistrator();

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
	}

	public class BonusRegistrator : Registrator<Bonus>
	{
		public event UnityAction<Bonus> onBonusChanged;

		public BonusRegistrator()
		{
			onItemAdded += OnBonusAdded;
			onItemRemoved += OnBonusRemoved;
		}

		private void OnBonusChanged(Bonus bonus)
		{
			onBonusChanged?.Invoke(bonus);
		}

		private void OnBonusAdded(Bonus bonus)
		{
			bonus.onChanged += OnBonusChanged;
		}

		private void OnBonusRemoved(Bonus bonus)
		{
			bonus.onChanged -= OnBonusChanged;
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
}