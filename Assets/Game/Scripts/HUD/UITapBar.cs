using Game.Entities;
using Game.Managers.ClickManager;
using Game.UI;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.HUD
{
    public class UITapBar : MonoBehaviour
    {
		[SerializeField] private UIAnimatedBar bar1;
		[SerializeField] private UIAnimatedBar bar2;

		private TapBar tapCount;

		private SignalBus signalBus;
		private Player player;

		[Inject]
		private void Construct(SignalBus signalBus, Player player)
		{
			this.signalBus = signalBus;
			this.player = player;
		}

		private void Start()
		{
			tapCount = player.PlayerSheet.TapBar;
			tapCount.onChanged += OnTapCountChanged;
			OnTapCountChanged();
		}

		private void OnDestroy()
		{
			tapCount.onChanged -= OnTapCountChanged;
		}

		private void OnTapCountChanged()
		{
			bar1.FillAmount = tapCount.PercentValue;
			bar1.BarText.text = $"{System.Math.Round(tapCount.PercentValue * 100)}%";
		}
	}
}