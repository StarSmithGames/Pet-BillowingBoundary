using Game.Managers.ClickManager;
using Game.Managers.StorageManager;
using Game.Systems.AnalyticsSystem;
using Game.Systems.LocalizationSystem;
using Game.Systems.MarketSystem;
using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

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

		private ClickStarter conveyor;
		private ISaveLoad saveLoad;
		private AnalyticsSystem analyticsSystem;

		[Inject]
		private void Construct(
			ClickStarter conveyor,
			ISaveLoad saveLoad,
			AnalyticsSystem analyticsSystem)
		{
			this.conveyor = conveyor;
			this.saveLoad = saveLoad;
			this.analyticsSystem = analyticsSystem;
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
			if (!isInitialized) return;

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

			signalBus?.Fire<SignalSave>();

			analyticsSystem.LogEvent_skill_upgraded(properties[index].ID);
		}

		private void Init()
		{
			if (!isInitialized)
			{
				isInitialized = true;

				Chance = new FireFistChanceProperty(0f);
				Duration = new FireFistDurationProperty(data.releaseDuration);
				Power = new FireFistPowerProperty(1f);//100% x2

				properties.Add(Chance);
				properties.Add(Duration);
				properties.Add(Power);

				if (saveLoad.GetStorage().IsFirstTime.GetData() == false)
				{
					var data =  saveLoad.GetStorage().Profile.GetData().playerData.fireFistData;

					Chance.SetLevel(data.chanceLevel);
					Duration.SetLevel(data.durationLevel);
					Power.SetLevel(data.powerLevel);

					onChanged?.Invoke(this);
				}
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

		public Data GetData()
		{
			return new Data()
			{
				chanceLevel = Chance.Level,
				durationLevel = Duration.Level,
				powerLevel = Power.Level,
			};
		}

		[System.Serializable]
		public class Data
		{
			public int chanceLevel;
			public int durationLevel;
			public int powerLevel;
		}
	}


	public class FireFistChanceProperty : SkillProperty
	{
		public override string ID => "fire_fist.chance";

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

		public override void SetLevel(int level)
		{
			base.SetLevel(level);

			CurrentValue += 0.01f * Level;
			XModifier.SetValue(TotalValue * 100f);//xTotalValue
		}

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

		protected override BFN Formule() => Level == 0 ? new BFN(200, 0) : BFN.FormuleExpoLevelLow(200, Level + 1);
	}

	public class FireFistDurationProperty : SkillProperty
	{
		public override string ID => "fire_fist.duration";

		public override string LocalizationKey => "ui.skill.fire_fist.duration";

		public FireFistDurationProperty(float value) : base(value) { }

		public override void SetLevel(int level)
		{
			base.SetLevel(level);

			CurrentValue += 0.5f * Level;
		}


		public override void LevelUp()
		{
			CurrentValue += 0.5f;
			base.LevelUp();
		}

		public override string GetOutput(LocalizationSystem localizationSystem)
		{
			return string.Format(base.GetOutput(localizationSystem), Math.Round(TotalValue, 2));
		}

		protected override BFN Formule() => Level == 0 ? new BFN(300, 0) : BFN.FormuleExpoLevelLow(300, Level + 1);
	}

	public class FireFistPowerProperty : SkillProperty
	{
		public override string ID => "fire_fist.power";

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

		public override void SetLevel(int level)
		{
			base.SetLevel(level);

			CurrentValue += 0.01f * Level;

			XModifier.SetValue(TotalValue * 100f);
		}

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

		protected override BFN Formule() => Level == 0 ? new BFN(450, 0) : BFN.FormuleExpoLevelLow(450, Level + 1);
	}
}

public enum TapPhase
{
	Accumulation,
	Decrease,
	Release,
}