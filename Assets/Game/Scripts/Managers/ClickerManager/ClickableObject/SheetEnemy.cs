using Game.Entities;

namespace Game.Managers.ClickManager
{
	public class SheetEnemy
	{
		public HealthPointsBar HealthPointsBar { get; }

		public SheetEnemy(TargetData data)
		{
			HealthPointsBar = new HealthPointsBar(new BFN(data.baseHealthPoints, 0).compressed, BFN.Zero, new BFN(data.baseHealthPoints, 0).compressed);
		}
	}
	public class HealthPointsBar : AttributeBFNBar
	{
		public HealthPointsBar(BFN currentValue, BFN minValue, BFN maxValue) : base(currentValue, minValue, maxValue) { }
	
		public void Resize(BFN max)
		{
			MaxValue = max;
			CurrentValue = max;
		}

		public void Resize(BFN value, BFN max)
		{
			MaxValue = max;
			CurrentValue = value;
		}
	}
}