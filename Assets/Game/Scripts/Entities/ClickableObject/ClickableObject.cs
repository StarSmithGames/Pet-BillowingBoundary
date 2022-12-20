using DG.Tweening;

using Game.Systems.FloatingSystem;
using Game.Systems.VFX;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Entities
{
	public abstract class ClickableObject : MonoBehaviour
	{
		//Muzzle effect
		//Impact effect

		public EnemyData Data => data;
		[HideLabel]
		[SerializeField] public EnemyData data;
		[Space]
		[SerializeField] public List<FloatingPoint> points = new List<FloatingPoint>();
		[SerializeField] public List<ParticleVFX> particles = new List<ParticleVFX>();
		[SerializeField] private Collider collider;
		[Space]
		[SerializeField] private PunchSettings smallPunch;

		public SheetEnemy Sheet
		{
			get
			{
				if(sheet == null)
				{
					sheet = new SheetEnemy(data);
				}

				return sheet;
			}
		}
		private SheetEnemy sheet;

		private void Start()
		{
			collider.enabled = false;
		}

		public Transform GetRandomPoint()
		{
			return points.RandomItem().transform;
		}

		public ParticleVFX GetRandomParticle()
		{
			return particles.RandomItem();
		}

		public void CustomPunch(PunchSettings settings)
		{
			transform.DORewind();
			transform.DOPunchScale(settings.GetPunch(), settings.duration, settings.vibrato, settings.elasticity);
		}

		[Button]
		public void SmallPunch()
		{
			CustomPunch(smallPunch);
		}

		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			points = GetComponentsInChildren<FloatingPoint>().ToList();
			particles = GetComponentsInChildren<ParticleVFX>().ToList();
			collider = GetComponentInChildren<Collider>();
		}
	}

	[System.Serializable]
	public class PunchSettings
	{
		public Punch punch;
		public float duration = 0.25f;
		public int vibrato = 10;
		public float elasticity = 1f;

		public Vector3 GetPunch()
		{
			if (punch.isPunchRandom)
			{
				return new Vector3(punch.randomXLimits.RandomBtw(), punch.randomYLimits.RandomBtw(), punch.randomZLimits.RandomBtw());
			}

			return punch.punch;
		}

		[System.Serializable]
		public class Punch
		{
			public bool isPunchRandom = false;
			[HideIf("isPunchRandom")]
			public Vector3 punch = Vector3.one;
			[ShowIf("isPunchRandom")]
			public Vector2 randomXLimits;
			[ShowIf("isPunchRandom")]
			public Vector2 randomYLimits;
			[ShowIf("isPunchRandom")]
			public Vector2 randomZLimits;
		}
	}
}