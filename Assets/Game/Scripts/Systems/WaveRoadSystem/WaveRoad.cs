using Game.Entities;
using Game.Managers.ClickManager;

using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.WaveRoadSystem
{
    public class WaveRoad
    {
		public ClickableObject CurrentTarget { get; private set; }

		public Wave CurrentWave { get; private set; }
        public Wave NextWave { get; private set; }

		private Dictionary<TargetData, ClickableObject> poolTargets = new Dictionary<TargetData, ClickableObject>();

		private WaveRoadPatternData data;

		public WaveRoad(WaveRoadPatternData data)
        {
            this.data = data;

			CurrentWave = new Wave(0);
			CurrentWave.onWaveEnded += OnWaveEnded;
			CurrentWave.SetData(GetWave());

			UpdateTarget();
		}

		public void NextTarget()
		{
			CurrentWave.TargetsBar.CurrentValue++;
			CurrentWave.NextTarget();
			UpdateTarget();
		}

		private void UpdateTarget()
		{
			if (poolTargets.TryGetValue(CurrentWave.CurrentTarget, out ClickableObject target))
			{
				CurrentTarget = target;
			}
			else
			{
				CurrentTarget = CreateTarget(CurrentWave.CurrentTarget.prefab);
				poolTargets.Add(CurrentWave.CurrentTarget, CurrentTarget);
			}
		}

		private WaveRoadData GetWave()
		{
			if(data.repeat == RepeatStyle.Simple)
			{
				return data.waves[CurrentWave.CurrentValue % data.waves.Count];
			}

			return data.waves.RandomItem();
		}

		private ClickableObject CreateTarget(ClickableObject prefab)
		{
			var obj = GameObject.Instantiate(prefab);
			obj.transform.localScale = Vector3.one;
			obj.Enable(false);

			return obj;
		}

		private void OnWaveEnded()
		{
			CurrentTarget.onDead += OnBossDead;
		}

		private void OnBossDead()
		{
			CurrentTarget.onDead -= OnBossDead;

			Debug.LogError("Boss dead " + CurrentWave.CurrentTarget.name);

			CurrentWave.SetData(GetWave());
			UpdateTarget();
		}
	}

	public class Wave : Attribute<int>
	{
		public UnityAction onWaveEnded;

		public Targets TargetsBar { get; private set; }

		public TargetData CurrentTarget { get; private set; }
		public List<TargetData> WaveTargets { get; private set; }

		private WaveRoadData data;

		public Wave(int currentValue) : base(currentValue)
		{
			TargetsBar = new Targets(0, 0, 5);
		}

		public void SetData(WaveRoadData data)
		{
			this.data = data;

			TargetsBar.MaxValue = data.middleRoad.countTargets;
			TargetsBar.CurrentValue = 0;
			WaveTargets = data.middleRoad.style == TargetStyle.Shuffle ? data.middleRoad.targets.Shuffle() : new List<TargetData>(data.middleRoad.targets);
			NextTarget();
		}

		public void NextTarget()
		{
			if (TargetsBar.CurrentValue == TargetsBar.MaxValue)
			{
				CurrentTarget = GetBoss();
				onWaveEnded?.Invoke();
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
				return WaveTargets[(int)TargetsBar.CurrentValue % WaveTargets.Count];
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