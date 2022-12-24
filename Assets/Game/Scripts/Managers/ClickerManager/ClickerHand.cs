using DG.Tweening;

using Game.Entities;
using Game.Managers.GameManager;
using Game.Systems.CameraSystem;
using Game.Systems.FloatingSystem;

using Sirenix.OdinInspector;

using UnityEngine;

using Zenject;

namespace Game.Managers.ClickManager
{
	public class ClickerHand : MonoBehaviour
	{
		[SerializeField] private HandSettings settings;
		[SerializeField] private Vector3 pointHit;
		[ReadOnly]
		[SerializeField] private Vector3 startPosition;
		[ReadOnly]
		[SerializeField] private Vector3 endPosition;
		[Space]
		[SerializeField] private ParticleSystem fire;

		private Sequence sequence;
		private ClickableObject clickable;
		private HealthPointsBar hpClickable;
		private Gold goldCount;
		private BFN goldForPunch, damageForPunch;

		private SignalBus signalBus;
		private Player player;
		private TargetHandler targetHandler;
		private FloatingSystem floatingSystem;
		private CameraSystem cameraSystem;

		[Inject]
		private void Construct(
			SignalBus signalBus,
			Player player,
			TargetHandler targetHandler,
			FloatingSystem floatingTextSystem,
			CameraSystem cameraSystem)
		{
			this.signalBus = signalBus;
			this.player = player;
			this.targetHandler = targetHandler;
			this.floatingSystem = floatingTextSystem;
			this.cameraSystem = cameraSystem;
		}

		private void Start()
		{
			endPosition = endPosition - (transform.TransformPoint(pointHit) - startPosition);

			signalBus?.Subscribe<SignalTargetChanged>(OnTargetChanged);
			signalBus.Subscribe<SignalGameStateChanged>(OnGameStateChanged);

			goldCount = player.Gold;
			player.onTapChanged += OnTapChanged;
			OnTapChanged();
			OnTargetChanged();
		}

		private void OnDestroy()
		{
			if(player != null)
			{
				player.onTapChanged -= OnTapChanged;
			}

			signalBus?.Unsubscribe<SignalTargetChanged>(OnTargetChanged);
			signalBus?.Unsubscribe<SignalGameStateChanged>(OnGameStateChanged);
		}

		public void Punch()
		{
			Kill();

			sequence = DOTween.Sequence();
			sequence
				.Append(transform.DOMove(endPosition, 0.1f))
				.OnComplete(() =>
				{

					hpClickable.CurrentValue -= damageForPunch;

					//Coins
					if (Random.value <= player.TapGoldChance.TotalValue)//isGoldChance
					{
						BFN totalGoldForPunch = goldForPunch;

						if (clickable.Data.isHasCoinsOnPunch)
						{
							totalGoldForPunch += clickable.Data.GetCoinsOnPunch();
						}

						if (totalGoldForPunch != BFN.Zero)
						{
							goldCount.CurrentValue += totalGoldForPunch;

							floatingSystem.CreateText(clickable.GetRandomPoint().position, $"+{totalGoldForPunch}", color: Color.yellow, type: AnimationType.BasicDamage);
							floatingSystem.CreateCoin3D(clickable.GetRandomPoint().position);
						}
					}

					floatingSystem.CreateText(clickable.GetRandomPoint().position, $"-{damageForPunch}", color: Color.red, type: AnimationType.BasicDamage);
					clickable.GetRandomParticle().Play();
					clickable.SmallPunch();
					cameraSystem.SmallestShake();
				});
		}

		public void Back()
		{
			Kill();

			sequence = DOTween.Sequence();
			sequence
				.Append(transform.DOMove(startPosition, 0.15f));
		}

		public void Kill()
		{
			if (sequence != null)
			{
				if (sequence.IsPlaying())
				{
					sequence.Kill(true);
				}
			}
		}

		public void EnableFireFist(bool trigger)
		{
			if (trigger)
			{
				fire.Play();
			}
			else
			{
				fire.Stop();
			}
		}

		private void OnTapChanged()
		{
			goldForPunch = (settings.goldForPunch + player.TapGold.TotalValue) * player.TapGoldMultiplier.TotalValue;
			damageForPunch = (settings.damageForPunch + player.TapDamage.TotalValue) * player.TapDamageMultiplier.TotalValue;
		}

		private void OnTargetChanged()
		{
			clickable = targetHandler.CurrentTarget;
			hpClickable = clickable.Sheet.HealthPointsBar;
		}

		private void OnGameStateChanged(SignalGameStateChanged signal)
		{
			if (signal.newGameState == GameState.None)
			{
				Back();
			}
		}
		

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (Application.isPlaying) return;

			startPosition = transform.position;
			Gizmos.color = Color.green;
			if (Physics.Raycast(transform.position, transform.up, out RaycastHit hit))
			{
				endPosition = hit.point;
			}
			else
			{
				Gizmos.color = Color.red;
			}
			Gizmos.DrawRay(transform.position, transform.up * 5);

			Gizmos.DrawSphere(transform.TransformPoint(pointHit), 0.08f);

			Gizmos.DrawSphere(startPosition, 0.05f);
			Gizmos.DrawSphere(endPosition, 0.05f);
		}
#endif
	}

	[System.Serializable]
	public class HandSettings
	{
		[Min(0)]
		public int damageForPunch = 1;
		[Min(0)]
		public int goldForPunch = 1;
	}
}