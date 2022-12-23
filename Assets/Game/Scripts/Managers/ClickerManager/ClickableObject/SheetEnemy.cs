namespace Game.Managers.ClickManager
{
	public class SheetEnemy
	{
		public HealthPointsBar HealthPointsBar { get; }

		public SheetEnemy(EnemyData data)
		{
			HealthPointsBar = new HealthPointsBar(data.baseHealthPoints.compressed, BFN.Zero, data.baseHealthPoints.compressed);
		}
	}
	public class HealthPointsBar : AttributeBFNBar
	{
		public HealthPointsBar(BFN currentValue, BFN minValue, BFN maxValue) : base(currentValue, minValue, maxValue) { }
	}
}