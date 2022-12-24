using Game.Managers.ClickManager;
using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.HUD
{
	public class UITargetBar : MonoBehaviour
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI TargetName { get; private set; }
		[SerializeField] private UIAnimatedBar bar;

		private HealthPointsBar hp;

		private SignalBus signalBus;
		private TargetHandler targetHandler;

		[Inject]
		private void Construct(SignalBus signalBus, TargetHandler targetHandler)
		{
			this.signalBus = signalBus;
			this.targetHandler = targetHandler;
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

			if (targetHandler.CurrentTarget != null)
			{
				TargetName.text = targetHandler.CurrentTarget.Data.information.name;
				hp = targetHandler.CurrentTarget.Sheet.HealthPointsBar;
				hp.onChanged += OnTapCountBarChanged;
				OnTapCountBarChanged();
			}
		}
	}
}