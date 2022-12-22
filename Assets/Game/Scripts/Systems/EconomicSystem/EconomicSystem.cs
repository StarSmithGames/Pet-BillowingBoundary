namespace Game.Systems.EconomicSystem
{
	public class EconomicSystem
	{
		public Economic CurrentEconomic => economics.common;

		private Economics economics;

		public EconomicSystem(Economics economics)
		{
			this.economics = economics;
		}
	}
}