using UnityEngine.Events;

namespace Game.Entities
{
	public class PlayerSheet
	{
		public TapBar TapBar { get; }

		public PlayerSheet()
		{
			
			TapBar = new TapBar(0, 0, 100);
		}

		public class Data
		{
			public int gold;
			public int tapCount;
		}
	}

	public class TapBar : AttributeBar
	{
		public event UnityAction onPhaseChanged;

		public TapPhase TapPhase
		{
			get => tapPhase;
			set
			{
				tapPhase = value;
				onPhaseChanged?.Invoke();
			}
		}
		private TapPhase tapPhase = TapPhase.Accumulation;

		public TapBar(float currentValue, float min, float max) : base(currentValue, min, max) { }

		public void Resize(float currentValue, float min, float max)
		{
			MinValue = min;
			MaxValue = max;
			CurrentValue = currentValue;
		}

		public void SetTapPhase(TapPhase tapPhase)
		{
			TapPhase = tapPhase;
		}
	}
}