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

		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			points = GetComponentsInChildren<FloatingPoint>().ToList();
			particles = GetComponentsInChildren<ParticleVFX>().ToList();
			collider = GetComponentInChildren<Collider>();
		}
	}
}