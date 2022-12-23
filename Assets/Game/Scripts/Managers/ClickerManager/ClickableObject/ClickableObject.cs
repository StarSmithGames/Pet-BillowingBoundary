using DG.Tweening;

using Game.Systems.FloatingSystem;
using Game.Systems.VFX;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Managers.ClickManager
{
	public abstract class ClickableObject : MonoBehaviour
	{
		public UnityAction onDead;

		public bool IsInitialized { get; private set; } = false;
		public bool IsEnabled { get; private set; } = true;

		public EnemyData Data => data;
		[SerializeField] public EnemyData data;
		[SerializeField] private PunchSettings smallPunch;
		[Header("Vars")]
		[ReadOnly]
		[SerializeField] public List<FloatingPoint> points = new List<FloatingPoint>();
		[ReadOnly]
		[SerializeField] public List<ParticleVFX> particles = new List<ParticleVFX>();
		[ReadOnly]
		public Collider Collider => collider;
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

		private bool isDead = false;
		private Vector3 startRotation;

		private void Start()
		{
			if (!IsInitialized)
			{
				Initialization();
			}
		}

		private void OnDestroy()
		{
			Sheet.HealthPointsBar.onChanged -= OnHealthPointsChanged;
		}

		public void Initialization(bool enable = false)
		{
			Enable(enable);

			Sheet.HealthPointsBar.onChanged += OnHealthPointsChanged;

			startRotation = transform.rotation.eulerAngles;

			IsInitialized = true;
		}

		public void Enable(bool trigger)
		{
			gameObject.SetActive(trigger);

			IsEnabled = trigger;
		}

		public void CustomPunch(PunchSettings settings)
		{
			transform.DORewind();
			transform.DOPunchScale(settings.GetPunch(), settings.duration, settings.vibrato, settings.elasticity);
		}

		public void SmallPunch()
		{
			CustomPunch(smallPunch);
		}

		[Button]
		public void Dead()
		{
			Sheet.HealthPointsBar.CurrentValue -= Sheet.HealthPointsBar.CurrentValue;
		}

		public Tween Flip()
		{
			Sequence sequence = DOTween.Sequence();
			sequence
				.Append(transform.DOMoveY(3f, 0.2f))
				.Append(transform.DORotate(transform.eulerAngles + new Vector3(0, 0, 180), 0.2f))
				.Join(transform.DOMoveY(0, 0.3f));

			return sequence;
		}
		public void Refresh()
		{
			transform.eulerAngles = startRotation;
			sheet.HealthPointsBar.CurrentValue = sheet.HealthPointsBar.MaxValue;
			isDead = false;
			Debug.LogError("Refresh");
		}

		public Transform GetRandomPoint()
		{
			return points.RandomItem().transform;
		}

		public ParticleVFX GetRandomParticle()
		{
			return particles.RandomItem();
		}

		private void OnHealthPointsChanged()
		{
			if (isDead) return;

			if(Sheet.HealthPointsBar.CurrentValue <= BFN.Zero)
			{
				isDead = true;
				onDead?.Invoke();
			}
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