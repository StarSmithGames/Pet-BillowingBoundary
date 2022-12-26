using Game.Managers.ClickManager;
using Game.Systems.LocalizationSystem;
using Game.UI;
using System;
using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public class FireFistSkill : ActiveSkill
	{
		public override SkillData Data => data;
		[SerializeField] private FireFistSkillData data;
		[SerializeField] private UIGradienRageEffect effect;

		public FireFistChanceProperty Chance { get; private set; }
		public FireFistDurationProperty Duration { get; private set; }
		public FireFistPowerProperty Power { get; private set; }

		private bool isInitialized = false;
		private TapBar tapBar;
		private float t = 0;

		private List<SkillProperty> properties = new List<SkillProperty>();
		private PercentModifier x2Modifier;

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

			x2Modifier = new PercentModifier(100f);//Add 100% == x2

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
				tapBar.CurrentValue = Mathf.Lerp(tapBar.MinValue, tapBar.MaxValue, 1f - (t / data.releaseDuration));

				if (tapBar.CurrentValue == 0)
				{
					tapBar.SetTapPhase(TapPhase.Accumulation);
					OnStartAccumulation();
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
				Power = new FireFistPowerProperty(2f);

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

			player.TapGoldMultiplier.RemoveModifier(x2Modifier);
			player.TapCriticalPower.RemoveModifier(x2Modifier);
		}

		private void OnStartRelease()
		{
			conveyor.CurrentLeftHand.EnableFireFist(true);
			conveyor.CurrentRightHand.EnableFireFist(true);
			effect.Show();

			player.TapGoldMultiplier.AddModifier(x2Modifier);
			player.TapCriticalPower.AddModifier(x2Modifier);
		}

		private void OnTapsChanged()
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


	public abstract class SkillProperty : AttributeModifiableFloat
	{
		public int Level { get; private set; } = 0;

		protected bool isInitialized = false;
		protected BFN currentCost;

		public SkillProperty(float value) : base(value) { }

		public virtual void LevelUp()
		{
			Level++;

			currentCost = Formule();
		}

		public virtual string GetOutput(LocalizationSystem localizationSystem)
		{
			return localizationSystem.Translate(LocalizationKey);
		}

		public BFN GetCost()
		{
			if (!isInitialized)
			{
				currentCost = Formule();
				isInitialized = true;
			}

			return currentCost;
		}

		protected abstract BFN Formule();
	}


	public class FireFistChanceProperty : SkillProperty
	{
		public override string LocalizationKey => "ui.skills.fire_fist.chance";

		public FireFistChanceProperty(float value) : base(value) { }

		public override void LevelUp()
		{
			base.LevelUp();
			CurrentValue += 0.01f;
		}

		public override string GetOutput(LocalizationSystem localizationSystem)
		{
			return string.Format(base.GetOutput(localizationSystem), Math.Round(TotalValue * 100f));
		}

		protected override BFN Formule() => Level == 0 ? new BFN(200, 0) : new BFN(Math.Ceiling(200 * (Mathf.Pow(1.07f, Level + 1))), 0).compressed;
	}

	public class FireFistDurationProperty : SkillProperty
	{
		public override string LocalizationKey => "ui.skills.fire_fist.duration";

		public FireFistDurationProperty(float value) : base(value) { }

		public override void LevelUp()
		{
			base.LevelUp();
			CurrentValue += 0.5f;
		}

		public override string GetOutput(LocalizationSystem localizationSystem)
		{
			return string.Format(base.GetOutput(localizationSystem), Math.Round(TotalValue, 2));
		}

		protected override BFN Formule() => Level == 0 ? new BFN(300, 0) : new BFN(Math.Ceiling(300 * (Mathf.Pow(1.07f, Level + 1))), 0).compressed;
	}

	public class FireFistPowerProperty : SkillProperty
	{
		public override string LocalizationKey => "ui.skills.fire_fist.power";

		public FireFistPowerProperty(float value) : base(value) { }

		public override void LevelUp()
		{
			base.LevelUp();
			CurrentValue += 0.01f;
		}

		public override string GetOutput(LocalizationSystem localizationSystem)
		{
			return string.Format(base.GetOutput(localizationSystem), Output);
		}

		protected override BFN Formule() => Level == 0 ? new BFN(450, 0) : new BFN(Math.Ceiling(450 * (Mathf.Pow(1.07f, Level + 1))), 0).compressed;
	}
}