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
		public TapDamageMultiplier TapDamageMultiplier { get; }
		public Gold Gold { get; }
		public TapGold TapGold { get; }
		public TapGoldMultiplier TapGoldMultiplier { get; }

		public Registrator<Bonus> BonusRegistrator { get; }

		public Player()
		{
			PlayerSheet = new PlayerSheet();

			Gold = new Gold(new BFN(1000, 0).compressed);
			TapGold = new TapGold(BFN.Zero);
			TapGoldMultiplier = new TapGoldMultiplier(1f);

			Taps = new Taps(0);
			TapDamage = new TapDamage(BFN.Zero);
			TapDamageMultiplier = new TapDamageMultiplier(1f);

			BonusRegistrator = new Registrator<Bonus>();

			TapGold.onChanged += OnTapChanged;
			TapGold.onModifiersChanged += OnTapChanged;
			TapGoldMultiplier.onChanged += OnTapChanged;
			TapGoldMultiplier.onModifiersChanged += OnTapChanged;

			TapDamage.onChanged += OnTapChanged;
			TapDamage.onModifiersChanged += OnTapChanged;
			TapDamageMultiplier.onChanged += OnTapChanged;
			TapDamageMultiplier.onModifiersChanged += OnTapChanged;
		}

		private void OnTapChanged()
		{
			onTapChanged?.Invoke();
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

	public class TapDamageMultiplier : AttributeModifiableFloat
	{
		public TapDamageMultiplier(float currentValue) : base(currentValue) { }
	}
	public class TapGold : AttributeModifiableBFN
	{
		public TapGold(BFN currentValue) : base(currentValue) { }
	}

	public class TapGoldMultiplier : AttributeModifiableFloat
	{
		public TapGoldMultiplier(float currentValue) : base(currentValue) { }
	}
	#endregion
}