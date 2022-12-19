using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
	public class Player
	{
		public ClickableObject CurrentTarget { get; private set; }

		public TapCount TapCount { get; }


		public Player()
		{
			TapCount = new TapCount(0);
		}

		public class Data
		{
			public int tapCount;

		}
	}

	public class PlayerSettings
	{

	}

	public class TapCount : Attribute
	{
		public override string LocalizationKey => "vars.tap_count";

		public TapCount(float currentValue) : base(currentValue) { }
	}
}