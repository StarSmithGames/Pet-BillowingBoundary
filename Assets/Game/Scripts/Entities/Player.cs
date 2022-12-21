namespace Game.Entities
{
	public class Player
	{
		public PlayerSheet PlayerSheet { get; }

		public Player()
		{
			PlayerSheet = new PlayerSheet();
		}
	}
}