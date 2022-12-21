using Game.Managers.ClickManager;
using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.HUD
{
	public class UITargetBar : MonoBehaviour
	{
		[SerializeField] private UIAnimatedBar bar;

		private HealthPointsBar hp;

		private SignalBus signalBus;
		private ClickerConveyor conveyor;

		[Inject]
		private void Construct(SignalBus signalBus, ClickerConveyor conveyor)
		{
			this.signalBus = signalBus;
			this.conveyor = conveyor;
		}

		private void Start()
		{
			signalBus?.Subscribe<SignalTargetChanged>(OnTargetChanged);
			OnTargetChanged();
		}

		private void OnDestroy()
		{
			hp.onChanged -= OnTapCountBarChanged;
			signalBus.Unsubscribe<SignalTargetChanged>(OnTargetChanged);
		}

		private void OnTapCountBarChanged()
		{
			bar.FillAmount = hp.PercentValue;
			bar.BarText.text = hp.Output;
		}

		private void OnTargetChanged()
		{
			if (hp != null)
			{
				hp.onChanged -= OnTapCountBarChanged;
			}

			if (conveyor.CurrentClickableObject != null)
			{
				hp = conveyor.CurrentClickableObject.Sheet.HealthPointsBar;
				hp.onChanged += OnTapCountBarChanged;
				OnTapCountBarChanged();
			}
		}
	}
}