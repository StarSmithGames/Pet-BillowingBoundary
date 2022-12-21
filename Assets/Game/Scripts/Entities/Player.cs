namespace Game.Entities
{
	public class Player
	{
		public PlayerSheet PlayerSheet { get; }

		public GoldMultiplier GoldMultiplier;
		public DamageMultiplier DamageMultiplier;

		public Player()
		{
			PlayerSheet = new PlayerSheet();

			GoldMultiplier = new GoldMultiplier(1f);
			DamageMultiplier = new DamageMultiplier(1f);
		}
	}

	public class GoldMultiplier : AttributeModifiable
	{
		public GoldMultiplier(float currentValue) : base(currentValue) { }
	}

	public class DamageMultiplier : AttributeModifiable
	{
		public DamageMultiplier(float currentValue) : base(currentValue) { }
	}
}