using UnityEngine.Events;

namespace Game.Entities
{
	public class Player
	{
		public UnityAction onTapChanged;

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

			Gold = new Gold(0);
			TapGold = new TapGold(1f);
			TapGoldMultiplier = new TapGoldMultiplier(1f);

			Taps = new Taps(0);
			TapDamage = new TapDamage(1f);
			TapDamageMultiplier = new TapDamageMultiplier(1f);

			BonusRegistrator = new Registrator<Bonus>();

			TapGold.onChanged += onTapChanged;
			TapGoldMultiplier.onChanged += onTapChanged;

			TapDamage.onChanged += onTapChanged;
			TapDamageMultiplier.onChanged += onTapChanged;
		}

		private void OnTapChanged()
		{
			onTapChanged?.Invoke();
		}
	}

	public class Gold : Attribute
	{
		public Gold(float currentValue) : base(currentValue) { }
	}

	#region Tap
	public class Taps : Attribute
	{
		public Taps(float currentValue) : base(currentValue) { }
	}
	public class TapDamage : AttributeModifiable
	{
		public TapDamage(float currentValue) : base(currentValue) { }
	}

	public class TapDamageMultiplier : AttributeModifiable
	{
		public TapDamageMultiplier(float currentValue) : base(currentValue) { }
	}
	public class TapGold : AttributeModifiable
	{
		public TapGold(float currentValue) : base(currentValue) { }
	}

	public class TapGoldMultiplier : AttributeModifiable
	{
		public TapGoldMultiplier(float currentValue) : base(currentValue) { }
	}
	#endregion
}