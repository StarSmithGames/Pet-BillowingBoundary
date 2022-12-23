using DG.Tweening;

using Game.Managers.GameManager;
using Game.Systems.FloatingSystem;

using System.Collections;

using UnityEngine;
using UnityEngine.Assertions;

using Zenject;

namespace Game.Managers.ClickManager
{
	public class TargetHandler : IInitializable
	{
		public ClickableObject CurrentTarget { get; private set; }
		public ClickableObject LastClickableObject { get; private set; }

		private Coroutine awardCoinsCoroutine = null;

		private SignalBus signalBus;
		private ClickerConveyor conveyor;
		private FloatingAwards floatingAwards;
		private GameManager.GameManager gameManager;
		private AsyncManager.AsyncManager asyncManager;

		public TargetHandler(SignalBus signalBus,
			ClickerConveyor clickerConveyor,
			FloatingAwards floatingAwards,
			GameManager.GameManager gameManager,
			AsyncManager.AsyncManager asyncManager)
		{
			this.signalBus = signalBus;
			this.conveyor = clickerConveyor;
			this.floatingAwards = floatingAwards;
			this.gameManager = gameManager;
			this.asyncManager = asyncManager;
		}

		public void Initialize()
		{
			asyncManager.StartCoroutine(Start());
		}

		private IEnumerator Start()
		{
			SpawnTarget();
			yield return new WaitForSeconds(1f);
			MoveTargetIn().Play();
		}

		private void SpawnTarget()
		{
			LastClickableObject = CurrentTarget;
			Despawn(LastClickableObject);
			CurrentTarget = Create();

			signalBus?.Fire(new SignalTargetChanged() { target = CurrentTarget });

			ClickableObject Create()
			{
				var obj = conveyor.CreateTarget();
				Assert.IsTrue(obj != null);
				if (!obj.IsInitialized)
				{
					obj.Initialization(true);
				}
				else
				{
					obj.Enable(true);
				}
				obj.onDead += OnTargetDead;
				obj.transform.SetParent(conveyor.ClickableContent);
				obj.transform.position = conveyor.GetRandomStartPosition();
				
				return obj;
			}

			void Despawn(ClickableObject obj)
			{
				if (obj != null)
				{
					obj.onDead -= OnTargetDead;
					obj.Enable(false);
					obj.transform.SetParent(conveyor.ClickableConveyor);
					obj.Refresh();
				}
			}
		}

		private IEnumerator ReplaceTarget()
		{
			yield return MoveTargetOut().WaitForCompletion();
			SpawnTarget();
			yield return MoveTargetIn().WaitForCompletion();
		}

		private Tween MoveTargetIn()
		{
			return CurrentTarget.transform
				.DOMove(Vector3.zero, 0.35f)
				.SetEase(Ease.OutBounce)
				.OnComplete(() =>
				{
					gameManager.ChangeState(GameState.Gameplay);
				});
		}

		private Tween MoveTargetOut()
		{
			return CurrentTarget.transform.DOMove(Vector3.left * 5f, 0.2f);
		}

		private void Next()
		{
			gameManager.ChangeState(GameState.None);

			if (awardCoinsCoroutine != null)//wait awards and next
			{
				CurrentTarget.Flip();
			}
			else//next
			{
				Sequence sequence = DOTween.Sequence();

				sequence
					.Append(CurrentTarget.Flip())
					.AppendCallback(() => asyncManager.StartCoroutine(ReplaceTarget()));
			}
		}

		private void OnTargetDead()
		{
			if (CurrentTarget.data.isHasCoinsAfterDefeat)
			{
				BFN totalCoins = CurrentTarget.data.GetCoinsAfterDefeat();

				awardCoinsCoroutine = floatingAwards.StartAwardCoins(Camera.main.WorldToScreenPoint(CurrentTarget.transform.position), 25, totalCoins,
				() =>
				{
					awardCoinsCoroutine = null;
					asyncManager.StartCoroutine(ReplaceTarget());
				});
			}
			Next();
		}
	}
}