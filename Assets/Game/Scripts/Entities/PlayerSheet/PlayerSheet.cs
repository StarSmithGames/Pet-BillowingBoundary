using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Entities
{
	public class PlayerSheet
	{
		public Gold Gold { get; }
		public Tap TapCount { get; }
		public TapBar TapBar { get; }

		public TapIncrement tapIncrement { get; }

		public PlayerSheet()
		{
			Gold = new Gold(0);
			TapCount = new Tap(0);
			TapBar = new TapBar(0, 0, 100);
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

	public class Tap : Attribute
	{
		public Tap(float currentValue) : base(currentValue) { }
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

			Debug.LogError(MaxValue);
		}

		public void SetTapPhase(TapPhase tapPhase)
		{
			TapPhase = tapPhase;
		}
	}

	public class TapIncrement : Attribute
	{
		public TapIncrement(float currentValue) : base(currentValue) { }
	}


	public enum TapPhase
	{
		Accumulation,
		Release,
	}
}