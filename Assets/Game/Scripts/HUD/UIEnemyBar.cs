using Game.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.HUD
{
	public class UIEnemyBar : UIBar
	{
		private TapCountBar tapCountBar;

		private ClickerConveyor conveyor;

		[Inject]
		private void Construct(ClickerConveyor conveyor)
		{
			this.conveyor = conveyor;
		}
		private void Start()
		{
			tapCountBar = conveyor.CurrentClickableObject.Sheet.TapCountBar;

			tapCountBar.onChanged += OnTapCountBarChanged;
			OnTapCountBarChanged();
		}

		private void OnDestroy()
		{
			tapCountBar.onChanged -= OnTapCountBarChanged;
		}

		private void OnTapCountBarChanged()
		{
			FillAmount = tapCountBar.PercentValue;
			BarText.text = tapCountBar.Output;
		}
	}
}