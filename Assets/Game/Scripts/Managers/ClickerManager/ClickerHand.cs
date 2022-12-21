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

		private SignalBus signalBus;
		private Player player;
		private ClickerConveyor clickerConveyor;
		private FloatingSystem floatingSystem;
		private CameraSystem cameraSystem;

		[Inject]
		private void Construct(
			SignalBus signalBus,
			Player player,
			ClickerConveyor clickerConveyor,
			FloatingSystem floatingTextSystem,
			CameraSystem cameraSystem)
		{
			this.signalBus = signalBus;
			this.player = player;
			this.clickerConveyor = clickerConveyor;
			this.floatingSystem = floatingTextSystem;
			this.cameraSystem = cameraSystem;
		}

		private void Start()
		{
			endPosition = endPosition - (transform.TransformPoint(pointHit) - startPosition);

			signalBus.Subscribe<SignalGameStateChanged>(OnGameStateChanged);
		}

		private void OnDestroy()
		{
			signalBus.Unsubscribe<SignalGameStateChanged>(OnGameStateChanged);
		}

		public void Punch()
		{
			Kill();

			sequence = DOTween.Sequence();
			sequence
				.Append(transform.DOMove(endPosition, 0.1f))
				.OnComplete(() =>
				{
					clickerConveyor.CurrentClickableObject.Sheet.HealthPointsBar.CurrentValue -= settings.damage;
					player.PlayerSheet.Gold.CurrentValue += settings.goldForPunch;
					floatingSystem.CreateText(clickerConveyor.CurrentClickableObject.GetRandomPoint().position, $"+{settings.goldForPunch}", type: AnimationType.BasicDamage);
					floatingSystem.CreateCoin(clickerConveyor.CurrentClickableObject.GetRandomPoint().position);
					clickerConveyor.CurrentClickableObject.GetRandomParticle().Play();
					clickerConveyor.CurrentClickableObject.SmallPunch();
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
		[Min(1)]
		public int damage = 1;
		[Min(1)]
		public int goldForPunch = 1;
	}
}