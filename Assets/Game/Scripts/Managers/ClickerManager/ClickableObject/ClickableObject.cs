using DG.Tweening;
using Game.Entities;
using Game.Systems.FloatingSystem;
using Game.Systems.VFX;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Managers.ClickManager
{
	public abstract partial class ClickableObject : MonoBehaviour
	{
		public UnityAction onPunched;
		public UnityAction onDead;

		public bool IsInitialized { get; private set; } = false;
		public bool IsEnabled { get; private set; } = true;

		public TargetData Data => data;
		[SerializeField] private TargetData data;
		[Header("Vars")]
		[ReadOnly]
		[SerializeField] public List<FloatingPoint> points = new List<FloatingPoint>();
		[ReadOnly]
		[SerializeField] public List<ParticleVFX> particles = new List<ParticleVFX>();
		[ReadOnly]
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
			if(collider != null)
			{
				collider.enabled = false;
			}

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

		public void Refresh()
		{
			transform.eulerAngles = startRotation;
			sheet.HealthPointsBar.CurrentValue = sheet.HealthPointsBar.MaxValue;
			isDead = false;
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

#if UNITY_EDITOR
		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			points = GetComponentsInChildren<FloatingPoint>().ToList();
			particles = GetComponentsInChildren<ParticleVFX>().ToList();
			collider = GetComponentInChildren<Collider>();
		}

		[Button(DirtyOnClick = true)]
		private void SaveTransform()
		{
			Data.initRotation = transform.localRotation;
			Data.initPosition = transform.localPosition;

			EditorUtility.SetDirty(Data);
			AssetDatabase.SaveAssets();
		}
#endif
	}

	public partial class ClickableObject
	{

		public void CustomPunch(PunchSettings settings)
		{
			transform.DORewind();
			transform.DOKill(true);
			transform.DOPunchScale(settings.GetPunch(), settings.duration, settings.vibrato, settings.elasticity);
		}

		public void SmallPunch()
		{
			CustomPunch(Data.smallPunch.settings);
			onPunched?.Invoke();
		}

		public Tween Flip()
		{
			Sequence sequence = DOTween.Sequence();
			return sequence
				.Append(transform.DOMoveY(3f, 0.2f))
				.Append(transform.DORotate(transform.eulerAngles + new Vector3(0, 0, 180), 0.2f))
				.Join(transform.DOMoveY(0, 0.3f));
		}

		public Transform GetRandomPoint()
		{
			return points.RandomItem().transform;
		}

		public ParticleVFX GetRandomParticle()
		{
			return particles.RandomItem();
		}
	}
}