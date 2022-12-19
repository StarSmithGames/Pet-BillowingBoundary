namespace Game.Entities
{
	public class Player
	{
		public Gold Gold { get; }
		public TapCount TapCount { get; }

		public Player()
		{
			Gold = new Gold(0);
			TapCount = new TapCount(0);
		}

		public Player(Data data)
		{
			Gold = new Gold(data.gold);
			TapCount = new TapCount(data.tapCount);
		}

		public class Data
		{
			public int gold;
			public int tapCount;
		}
	}

	public class Gold : Attribute
	{
		public override string LocalizationKey => "vars.gold";

		public Gold(float currentValue) : base(currentValue) { }
	}

	public class TapCount : Attribute
	{
		public override string LocalizationKey => "vars.tap_count";

		public TapCount(float currentValue) : base(currentValue) { }
	}
}