using Game.Entities;
using Game.Managers.ClickManager;
using Game.Managers.StorageManager;

using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.WaveRoadSystem
{
    public sealed class WaveRoad
    {
		public event UnityAction onChanged;

		public ClickableObject CurrentTarget { get; private set; }

		public Wave CurrentWave { get; private set; }
        public Wave NextWave { get; private set; }

		private Dictionary<TargetData, ClickableObject> poolTargets = new Dictionary<TargetData, ClickableObject>();

		private Data data;
		private Player player;
		private Conveyor conveyor;
		private AnalyticsSystem.AnalyticsSystem analyticsSystem;

		public WaveRoad(WaveRoadPatternData pattern,
			ISaveLoad saveLoad,
			Player player,
			Conveyor conveyor,
			AnalyticsSystem.AnalyticsSystem analyticsSystem)
		{
			this.player = player;
			this.conveyor = conveyor;
			this.analyticsSystem = analyticsSystem;

			if (saveLoad.GetStorage().IsFirstTime.GetData() == false)
			{
				data = saveLoad.GetStorage().Profile.GetData().waveRoadData;

				CurrentWave = new Wave(data.wave);
			}
			else
			{
				data = new Data()
				{
					pattern = pattern,
				};

				CurrentWave = new Wave(0);
			}

			CurrentWave.SetData(GetWave());
			UpdateTarget();
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
			}

			CurrentTarget.onDead += OnTargetDead;
		}

		private WaveRoadData GetWave()
		{
			if(data.pattern.repeat == RepeatStyle.Simple)
			{
				return data.pattern.waves[CurrentWave.CurrentValue % data.pattern.waves.Count];
			}

			return data.pattern.waves.RandomItem();
		}

		private ClickableObject CreateTarget(ClickableObject prefab)
		{
			var obj = GameObject.Instantiate(prefab);
			obj.transform.localScale = Vector3.one;
			obj.Enable(false);

			return obj;
		}

		private void OnTargetDead()
		{
			CurrentTarget.onDead -= OnTargetDead;

			player.TargetsDefeat.CurrentValue++;

			analyticsSystem.LogEvent_target_defeat();

			if (CurrentWave.IsCompleted)
			{
				analyticsSystem.LogEvent_boss_defeat();

				player.BossesDefeat.CurrentValue++;

				CurrentWave.SetData(GetWave());
				CurrentWave.CurrentValue++;
				analyticsSystem.LogEvent_wave_completed();
				UpdateTarget();
			}
			else
			{
				CurrentWave.MiddleTargetsBar.CurrentValue++;
			}

			onChanged?.Invoke();
		}

		public Data GetData()
		{
			return new Data()
			{
				pattern = data.pattern,
				wave = CurrentWave.CurrentValue,
				targetsCount = (int)CurrentWave.MiddleTargetsBar.MaxValue,
				targetIndex = (int)CurrentWave.MiddleTargetsBar.CurrentValue,
				target = CurrentWave.CurrentTarget,
				targetData = CurrentTarget.GetData(),
			};
		}

		public class Data
		{
			public WaveRoadPatternData pattern;
			public int wave;
			public int targetsCount;
			public int targetIndex;
			public TargetData target;
			public ClickableObject.Data targetData;
		}
	}

	public class Wave : Attribute<int>
	{
		public bool IsCompleted { get; private set; } = false;

		public Targets MiddleTargetsBar { get; private set; }

		public TargetData CurrentTarget { get; private set; }
		public List<TargetData> WaveTargets { get; private set; }

		private WaveRoadData data;

		public Wave(int currentValue) : base(currentValue)
		{
			MiddleTargetsBar = new Targets(0, 0, 5);
		}

		public void SetData(WaveRoadData data)
		{
			this.data = data;

			IsCompleted = false;

			MiddleTargetsBar.MaxValue = data.middleRoad.countTargets;
			MiddleTargetsBar.CurrentValue = 0;
			WaveTargets = data.middleRoad.style == TargetStyle.Shuffle ? data.middleRoad.targets.Shuffle() : new List<TargetData>(data.middleRoad.targets);
			NextTarget();
		}

		public void NextTarget()
		{
			if (MiddleTargetsBar.CurrentValue == MiddleTargetsBar.MaxValue)
			{
				CurrentTarget = GetBoss();
				IsCompleted = true;
			}
			else
			{
				CurrentTarget = GetTarget();
			}
		}

		private TargetData GetTarget()
		{
			if (data.middleRoad.style == TargetStyle.Simple)
			{
				return WaveTargets[(int)MiddleTargetsBar.CurrentValue % WaveTargets.Count];
			}

			return WaveTargets.RandomItem();
		}

		private TargetData GetBoss()
		{
			if (data.endRoad.isHasBoss)
			{
				if (data.endRoad.isRandomBoss)
				{
					return data.endRoad.bosses.RandomItem();
				}

				return data.endRoad.boss;
			}

			return null;
		}
	}

	public class Targets : AttributeBar
	{
		public Targets(float value, float min, float max) : base(value, min, max) { }
	}
}