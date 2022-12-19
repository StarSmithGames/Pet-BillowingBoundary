using Game.Entities;

public class SheetEnemy
{
	public TapCountBar TapCountBar { get; }

	public SheetEnemy(EnemyData data)
	{
		TapCountBar = new TapCountBar(data.tapCount, 0, data.tapCount);
	}
}
public class TapCountBar : AttributeBar
{
	public override string LocalizationKey => "vars.tap_count";

	public TapCountBar(float currentValue, float minValue, float maxValue) : base(currentValue, minValue, maxValue) { }
}