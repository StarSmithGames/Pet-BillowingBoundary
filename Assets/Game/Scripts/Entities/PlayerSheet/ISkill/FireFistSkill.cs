using Game.Managers.ClickManager;
using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public class FireFistSkill : ActiveSkill
	{
		public override SkillData Data => data;
		[SerializeField] private FireFistSkillData data;
		[SerializeField] private UIGradienRageEffect effect;

		private TapBar tapBar;
		private float t = 0;

		private PercentModifier x2Modifier;

		private Player player;
		private ClickerConveyor conveyor;

		[Inject]
		private void Construct(Player player, ClickerConveyor conveyor)
		{
			this.player = player;
			this.conveyor = conveyor;
		}

		protected override void Start()
		{
			base.Start();

			x2Modifier = new PercentModifier(100f);//Add 100% == x2

			tapBar = player.PlayerSheet.TapBar;
			tapBar.Resize(0, 0, 100);
			player.PlayerSheet.TapCount.onChanged += OnTapCountChanged;
		}

		private void OnDestroy()
		{
			player.PlayerSheet.TapCount.onChanged -= OnTapCountChanged;
		}

		protected override void Update()
		{
			base.Update();

			if (tapBar.TapPhase == TapPhase.Release)
			{
				t += Time.deltaTime;
				tapBar.CurrentValue = Mathf.Lerp(tapBar.MinValue, tapBar.MaxValue, 1f - (t / data.releaseDuration));

				if (tapBar.CurrentValue == 0)
				{
					tapBar.SetTapPhase(TapPhase.Accumulation);
					OnStartAccumulation();
					t = 0;
				}
			}
		}

		private void OnStartAccumulation()
		{
			conveyor.CurrentLeftHand.EnableFireFist(false);
			conveyor.CurrentRightHand.EnableFireFist(false);
			effect.Hide();

			player.GoldMultiplier.RemoveModifier(x2Modifier);
			player.DamageMultiplier.RemoveModifier(x2Modifier);
		}

		private void OnStartRelease()
		{
			conveyor.CurrentLeftHand.EnableFireFist(true);
			conveyor.CurrentRightHand.EnableFireFist(true);
			effect.Show();

			player.GoldMultiplier.AddModifier(x2Modifier);
			player.DamageMultiplier.AddModifier(x2Modifier);
		}

		private void OnTapCountChanged()
		{
			if (isHasCooldown && isCooldown) return;

			if (tapBar.TapPhase == TapPhase.Accumulation)
			{
				tapBar.CurrentValue += data.incrementForTap;

				if (tapBar.CurrentValue == tapBar.MaxValue)
				{
					tapBar.SetTapPhase(TapPhase.Release);
					OnStartRelease();
				}
			}
		}
	}
}