using Game.Entities;
using Game.Managers.AudioManager;
using Game.Managers.ClickManager;
using Game.Managers.StorageManager;
using Game.Managers.VibrationManager;
using System.Collections.Generic;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.WaveRoadSystem
{
    public sealed class WaveRoad
    {
		public event UnityAction onChanged;

		public ClickableObject CurrentTarget { get; private set; }

		public Wave CurrentWave { get; private set; }
        public Wave NextWave { get; private set; }

		private Dictionary<TargetData, ClickableObject> poolTargets = new Dictionary<TargetData, ClickableObject>();

		private SignalBus signalBus;
		private WaveRoadPatternData pattern;
		private ISaveLoad saveLoad;
		private Player player;
		private Conveyor conveyor;
		private AnalyticsSystem.AnalyticsSystem analyticsSystem;
		private AudioManager audioManager;
		private VibrationManager vibrationManager;

		public WaveRoad(
			SignalBus signalBus,
			WaveRoadPatternData pattern,
			ISaveLoad saveLoad,
			Player player,
			Conveyor conveyor,
			AnalyticsSystem.AnalyticsSystem analyticsSystem,
			AudioManager audioManager,
			VibrationManager vibrationManager)
		{
			this.saveLoad = saveLoad;
			this.player = player;
			this.conveyor = conveyor;
			this.analyticsSystem = analyticsSystem;
			this.audioManager = audioManager;
			this.vibrationManager = vibrationManager;

			if (saveLoad.GetStorage().IsFirstTime.GetData() == false)
			{
				var data = saveLoad.GetStorage().Profile.GetData().waveRoadData;
				this.pattern = data.pattern;
				CurrentWave = new Wave(data.lastWave);
				UpdateTarget();
				CurrentTarget.Sheet.HealthPointsBar.CurrentValue = data.lastClickable.hp;
			}
			else
			{
				this.pattern = pattern;
				CurrentWave = new Wave(GetWave());
				UpdateTarget();
			}
		}

		public void NextTarget()
		{
			CurrentWave.NextTarget();
			UpdateTarget();

			onChanged?.Invoke();
		}


		private void UpdateTarget()
		{
			if (poolTargets.TryGetValue(CurrentWave.CurrentTarget, out ClickableObject target))
			{
				CurrentTarget = target;
			}
			else
			{
				CurrentTarget = conveyor.objects.Find((x) => x.TargetData == CurrentWave.CurrentTarget);
				poolTargets.Add(CurrentWave.CurrentTarget, CurrentTarget);
				CurrentTarget.onDead += OnTargetDead;
			}
		}

		private WaveRoadData GetWave()
		{
			if(pattern.repeat == RepeatStyle.Simple)
			{
				return pattern.waves[CurrentWave.CurrentValue % pattern.waves.Count];
			}

			return pattern.waves.RandomItem();
		}

		private void OnTargetDead(ClickableObject clickableObject)
		{
			if (CurrentTarget != clickableObject) return;

			player.TargetsDefeat.CurrentValue++;

			analyticsSystem.LogEvent_target_defeat();

			if (CurrentWave.IsCompleted)//boss defeated
			{
				audioManager.PlayBossDefeated();
				vibrationManager.Vibrate(MoreMountains.NiceVibrations.HapticTypes.Success);

				analyticsSystem.LogEvent_boss_defeat();

				player.BossesDefeat.CurrentValue++;

				CurrentWave.SetWave(GetWave());
				CurrentWave.CurrentValue++;
				analyticsSystem.LogEvent_wave_completed();
				UpdateTarget();
			}
			else//target defeated
			{
				audioManager.PlayTargetDefeated();
				vibrationManager.Vibrate(MoreMountains.NiceVibrations.HapticTypes.LightImpact);

				CurrentWave.MiddleTargetsBar.CurrentValue++;
			}

			onChanged?.Invoke();
		}

		public Data GetData()
		{
			return new Data()
			{
				pattern = pattern,
				lastClickable = CurrentTarget.GetData(),
				lastWave = CurrentWave.GetData(),
			};
		}

		[System.Serializable]
		public class Data
		{
			public WaveRoadPatternData pattern;
			public ClickableObject.Data lastClickable;
			public Wave.Data lastWave;
		}
	}

	public class Wave : Attribute<int>
	{
		public bool IsCompleted => MiddleTargetsBar.CurrentValue == MiddleTargetsBar.MaxValue;

		public Targets MiddleTargetsBar { get; private set; }

		public TargetData CurrentTarget { get; private set; }
		public List<TargetData> WaveTargets { get; private set; }

		private WaveRoadData waveRoad;

		public Wave(WaveRoadData data) : base(0)//first time
		{
			MiddleTargetsBar = new Targets(0, 0, data.middleRoad.countTargets);
			SetWaveRoad(data);
			NextTarget();
		}

		public Wave(Data data) : base(data.waveCount)
		{
			MiddleTargetsBar = new Targets(data.targetCount, 0, data.waveRoadData.middleRoad.countTargets);
			SetWaveRoad(data.waveRoadData);
			CurrentTarget = data.targetData;
		}

		public void SetWave(WaveRoadData data)
		{
			MiddleTargetsBar.MaxValue = data.middleRoad.countTargets;
			MiddleTargetsBar.CurrentValue = 0;

			SetWaveRoad(data);
			NextTarget();
		}

		public void NextTarget()
		{
			if (IsCompleted)
			{
				CurrentTarget = GetBoss();
			}
			else
			{
				CurrentTarget = GetTarget();
			}
		}

		private void SetWaveRoad(WaveRoadData data)
		{
			waveRoad = data;

			WaveTargets = waveRoad.middleRoad.style == TargetStyle.Shuffle ? waveRoad.middleRoad.targets.Shuffle() : new List<TargetData>(waveRoad.middleRoad.targets);
		}

		private TargetData GetTarget()
		{
			if (waveRoad.middleRoad.style == TargetStyle.Simple)
			{
				return WaveTargets[(int)MiddleTargetsBar.CurrentValue % WaveTargets.Count];
			}

			return WaveTargets.RandomItem();
		}

		private TargetData GetBoss()
		{
			if (waveRoad.endRoad.isHasBoss)
			{
				if (waveRoad.endRoad.isRandomBoss)
				{
					return waveRoad.endRoad.bosses.RandomItem();
				}

				return waveRoad.endRoad.boss;
			}

			return null;
		}

		public Data GetData()
		{
			return new Data()
			{
				waveCount = CurrentValue,
				targetCount = (int)MiddleTargetsBar.CurrentValue,

				waveRoadData = waveRoad,
				targetData = CurrentTarget,
			};
		}

		[System.Serializable]
		public class Data
		{
			public int waveCount;
			public int targetCount;

			public WaveRoadData waveRoadData;
			public TargetData targetData;
		}
	}

	public class Targets : AttributeBar
	{
		public Targets(float value, float min, float max) : base(value, min, max) { }
	}
}