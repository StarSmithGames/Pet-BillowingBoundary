using Game.Managers.ClickManager;
using Game.Systems.LocalizationSystem;
using Game.Systems.MarketSystem;
using Game.UI;
using System;
using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public class FireFistSkill : ActiveSkill
	{
		public override SkillData SkillData => data;
		[SerializeField] private FireFistSkillData data;
		[SerializeField] private UIGradienRageEffect effect;

		public override bool IsUnknow { get; protected set; } = false;
		public override BuyType BuyType { get; protected set; } = BuyType.None;

		public FireFistChanceProperty Chance { get; private set; }
		public FireFistDurationProperty Duration { get; private set; }
		public FireFistPowerProperty Power { get; private set; }

		private bool isInitialized = false;
		private TapBar tapBar;
		private float t = 0;

		private List<SkillProperty> properties = new List<SkillProperty>();

		private ClickerConveyor conveyor;

		[Inject]
		private void Construct(ClickerConveyor conveyor)
		{
			this.conveyor = conveyor;
		}

		protected override void Start()
		{
			base.Start();

			Init();

			tapBar = player.PlayerSheet.TapBar;
			tapBar.Resize(0, 0, 100);//100 hits
			player.Taps.onChanged += OnTapsChanged;
		}

		private void OnDestroy()
		{
			player.Taps.onChanged -= OnTapsChanged;
		}

		protected override void Update()
		{
			base.Update();

			if (tapBar.TapPhase == TapPhase.Release)
			{
				t += Time.deltaTime;
				tapBar.CurrentValue = Mathf.Lerp(tapBar.MinValue, tapBar.MaxValue, 1f - (t / Duration.TotalValue));

				if (tapBar.CurrentValue == 0)
				{
					tapBar.SetTapPhase(TapPhase.Accumulation);
					OnStartAccumulation();
					t = 0;
				}
			}
			else if(tapBar.TapPhase == TapPhase.Accumulation)
			{
				t += Time.deltaTime;

				if(t >= data.waitingTime)
				{
					tapBar.SetTapPhase(TapPhase.Decrease);
					OnStartDecrease();
					t = 0;
				}
			}
			else if(tapBar.TapPhase == TapPhase.Decrease)
			{
				if(tapBar.CurrentValue != tapBar.MinValue)
				{
					t += Time.deltaTime;
					tapBar.CurrentValue -= Time.deltaTime * data.decreaseCurve.Evaluate(t / data.waitingTime) * data.decreaseSpeed;
				}
				else
				{
					t = 0;
				}
			}
		}

		public override SkillProperty GetProperty(int index)
		{
			Init();

			return properties[index];
		}

		public override void PurchaseProperty(int index)
		{
			properties[index].LevelUp();

			onChanged?.Invoke(this);
		}

		private void Init()
		{
			if (!isInitialized)
			{
				Chance = new FireFistChanceProperty(0f);
				Duration = new FireFistDurationProperty(data.releaseDuration);
				Power = new FireFistPowerProperty(1f);//100% x2

				properties.Add(Chance);
				properties.Add(Duration);
				properties.Add(Power);

				isInitialized = true;
			}
		}

		private void OnStartAccumulation()
		{
			conveyor.CurrentLeftHand.EnableFireFist(false);
			conveyor.CurrentRightHand.EnableFireFist(false);
			effect.Hide();

			player.TapGoldMultiplier.RemoveModifier(Power.XModifier);
			player.TapCriticalPower.RemoveModifier(Power.XModifier);
			player.TapDamage.RemoveModifier(Power.XModifier);

			player.TapGoldChance.RemoveModifier(Chance.XModifier);
			player.TapCriticalChance.RemoveModifier(Chance.XModifier);
		}

		private void OnStartDecrease()
		{

		}

		private void OnStartRelease()
		{
			conveyor.CurrentLeftHand.EnableFireFist(true);
			conveyor.CurrentRightHand.EnableFireFist(true);
			effect.Show();

			player.TapGoldMultiplier.AddModifier(Power.XModifier);
			player.TapCriticalPower.AddModifier(Power.XModifier);
			player.TapDamage.AddModifier(Power.XModifier);

			player.TapGoldChance.AddModifier(Chance.XModifier);
			player.TapCriticalChance.AddModifier(Chance.XModifier);
		}

		private void OnTapsChanged()
		{
			if (isHasCooldown && isCooldown) return;

			if (tapBar.TapPhase == TapPhase.Decrease)
			{
				t = 0;

				tapBar.SetTapPhase(TapPhase.Accumulation);
			}

			if (tapBar.TapPhase == TapPhase.Accumulation)
			{
				t = 0;//reset waitingTime

				tapBar.CurrentValue += data.incrementForTap;

				if (tapBar.CurrentValue == tapBar.MaxValue)
				{
					tapBar.SetTapPhase(TapPhase.Release);
					OnStartRelease();
				}
			}
		}
	}

	public class FireFistChanceProperty : SkillProperty
	{
		public override string LocalizationKey => "ui.skill.fire_fist.chance";

		public PercentModifier XModifier
		{
			get
			{
				if (xModifier == null)
				{
					xModifier = new PercentModifier(TotalValue * 100f);
				}

				return xModifier;
			}
		}
		private PercentModifier xModifier;

		public FireFistChanceProperty(float value) : base(value) { }

		public override void LevelUp()
		{
			CurrentValue += 0.01f;
			base.LevelUp();

			XModifier.SetValue(TotalValue * 100f);//xTotalValue
		}

		public override string GetOutput(LocalizationSystem localizationSystem)
		{
			return string.Format(base.GetOutput(localizationSystem), Math.Round(TotalValue * 100f));
		}

		protected override BFN Formule() => Level == 0 ? new BFN(200, 0) : new BFN(Math.Ceiling(200 * (Mathf.Pow(1.07f, Level + 1))), 0).compressed;
	}

	public class FireFistDurationProperty : SkillProperty
	{
		public override string LocalizationKey => "ui.skill.fire_fist.duration";

		public FireFistDurationProperty(float value) : base(value) { }

		public override void LevelUp()
		{
			CurrentValue += 0.5f;
			base.LevelUp();
		}

		public override string GetOutput(LocalizationSystem localizationSystem)
		{
			return string.Format(base.GetOutput(localizationSystem), Math.Round(TotalValue, 2));
		}

		protected override BFN Formule() => Level == 0 ? new BFN(300, 0) : new BFN(Math.Ceiling(300 * (Mathf.Pow(1.07f, Level + 1))), 0).compressed;
	}

	public class FireFistPowerProperty : SkillProperty
	{
		public override string LocalizationKey => "ui.skill.fire_fist.power";

		public PercentModifier XModifier
		{
			get
			{
				if(xModifier == null)
				{
					xModifier = new PercentModifier(TotalValue * 100f);
				}

				return xModifier;
			}
		}
		private PercentModifier xModifier;

		public FireFistPowerProperty(float value) : base(value) { }

		public override void LevelUp()
		{
			CurrentValue += 0.01f;
			base.LevelUp();

			XModifier.SetValue(TotalValue * 100f);
		}

		public override string GetOutput(LocalizationSystem localizationSystem)
		{
			return string.Format(base.GetOutput(localizationSystem), Math.Round(TotalValue + 1f, 2));
		}

		protected override BFN Formule() => Level == 0 ? new BFN(450, 0) : new BFN(Math.Ceiling(450 * (Mathf.Pow(1.07f, Level + 1))), 0).compressed;
	}
}

public enum TapPhase
{
	Accumulation,
	Decrease,
	Release,
}