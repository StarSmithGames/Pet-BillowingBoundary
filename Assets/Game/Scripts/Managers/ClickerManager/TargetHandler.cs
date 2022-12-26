using DG.Tweening;

using Game.HUD;
using Game.Managers.GameManager;
using Game.Systems.FloatingSystem;

using System.Collections;

using UnityEngine;
using UnityEngine.Assertions;

using Zenject;

namespace Game.Managers.ClickManager
{
	public partial class TargetHandler : IInitializable
	{
		public ClickableObject CurrentTarget { get; private set; }
		public ClickableObject LastClickableObject { get; private set; }

		private Coroutine awardCoinsCoroutine = null;
		private Coroutine targetIdleAnimation = null;

		private SignalBus signalBus;
		private TargetSettings targetSettings;
		private ClickerConveyor conveyor;
		private FloatingSystem floatingSystem;
		private FloatingAwards floatingAwards;
		private GameManager.GameManager gameManager;
		private AsyncManager.AsyncManager asyncManager;

		public TargetHandler(SignalBus signalBus,
			TargetSettings targetSettings,
			ClickerConveyor clickerConveyor,
			FloatingSystem floatingSystem,
			FloatingAwards floatingAwards,
			GameManager.GameManager gameManager,
			AsyncManager.AsyncManager asyncManager)
		{
			this.signalBus = signalBus;
			this.targetSettings = targetSettings;
			this.conveyor = clickerConveyor;
			this.floatingSystem = floatingSystem;
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
			targetIdleAnimation = asyncManager.StartCoroutine(TargetIdleAnimatinPingPong());
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
				obj.onPunched += OnTargetPunched;
				obj.onDead += OnTargetDead;
				obj.transform.SetParent(conveyor.TargetContent);
				obj.transform.localScale = Vector3.one;
				obj.transform.position = conveyor.GetRandomStartPosition();
				
				return obj;
			}

			void Despawn(ClickableObject obj)
			{
				if (obj != null)
				{
					obj.onPunched -= OnTargetPunched;
					obj.onDead -= OnTargetDead;
					obj.Enable(false);
					obj.transform.SetParent(conveyor.ClickableConveyor);
					obj.transform.localScale = Vector3.one;
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
				.DOLocalMove(new Vector3(0, 1, 0), 0.35f)
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
			if (CurrentTarget.Data.isHasCoinsAfterDefeat)
			{
				BFN totalCoins = CurrentTarget.Data.GetCoinsAfterDefeat();

				awardCoinsCoroutine = floatingAwards.StartAwardCoins(Camera.main.WorldToScreenPoint(CurrentTarget.transform.position), 10, totalCoins,
				() =>
				{
					awardCoinsCoroutine = null;
					asyncManager.StartCoroutine(ReplaceTarget());

					floatingSystem.CreateTextUI(UIGoldHUD.Instance.Count.transform.position, $"+{totalCoins.ToStringPritty()}", Color.green, AnimationType.AddGold);
					UIGoldHUD.Instance.Punch();
				});
			}
			Next();
		}
	}

	public partial class TargetHandler
	{
		private bool resetIdleAnimation = false;

		private IEnumerator TargetIdleAnimatinPingPong()
		{
			var targetContent = conveyor.TargetContent;

			bool direction = true;
			float t = 0;
			var left = Quaternion.Euler(targetSettings.left);
			var right = Quaternion.Euler(targetSettings.right);

			while (true)
			{
				if (resetIdleAnimation)
				{
					t = 0.5f;
					resetIdleAnimation = false;
				}

				targetContent.rotation = Quaternion.Lerp(left, right, targetSettings.curve.Evaluate(t));

				if (direction)
				{
					t += Time.deltaTime * targetSettings.speed;
				}
				else
				{
					t -= Time.deltaTime * targetSettings.speed;
				}

				if (t >= 1)
				{
					direction = false;
				}
				else if( t <= 0)
				{
					direction = true;
				}

				yield return null;
			}
		}
		
		private void OnTargetPunched()
		{
			resetIdleAnimation = true;
		}
	}

	[System.Serializable]
	public class TargetSettings
	{
		[Min(0.01f)]
		public float speed = 1f;
		public AnimationCurve curve;

		public Vector3 left = new Vector3(0, 0, -15);
		public Vector3 right = new Vector3(0, 0, 15);
	}
}