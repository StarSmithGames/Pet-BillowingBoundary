namespace Game.Managers.ClickManager
{
	public class SheetEnemy
	{
		public HealthPointsBar HealthPointsBar { get; }

		public SheetEnemy(EnemyData data)
		{
			HealthPointsBar = new HealthPointsBar(data.tapCount, 0, data.tapCount);
		}

		public void Refresh()
		{
			HealthPointsBar.CurrentValue = HealthPointsBar.MaxValue;
		}
	}
	public class HealthPointsBar : AttributeBar
	{
		public HealthPointsBar(float currentValue, float minValue, float maxValue) : base(currentValue, minValue, maxValue) { }
	}
}