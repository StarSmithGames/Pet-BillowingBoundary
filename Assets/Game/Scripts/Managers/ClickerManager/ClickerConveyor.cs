using DG.Tweening;

using Game.Entities;
using Game.Managers.ClickManager;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

using Zenject;
using Unity.VisualScripting;
using Game.Managers.GameManager;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Managers.ClickManager
{
	public class ClickerConveyor : MonoBehaviour
	{
		public ClickableObject CurrentClickableObject { get; private set; }

		[Header("Components")]
		[SerializeField] private Transform clickableContent;
		[SerializeField] private Transform clickableConveyor;
		[Space]
		[SerializeField] private ClickerHand clickerLeftHand;
		[SerializeField] private ClickerHand clickerRightHand;
		[Header("Vars")]
		[OnValueChanged("Refresh", true)]
		[SerializeField] private ConveyorSettings settings;
		[ReadOnly]
		[SerializeField] private List<ClickableObject> clickableObjects = new List<ClickableObject>();

		private int currentIndex = -1;

		private SignalBus signalBus;
		private Player player;
		private GameManager.GameManager gameManager;

		[Inject]
		private void Construct(SignalBus signalBus, Player player, GameManager.GameManager gameManager)
		{
			this.signalBus = signalBus;
			this.player = player;
			this.gameManager = gameManager;
		}

		private IEnumerator Start()
		{
			signalBus?.Subscribe<SignalTouchChanged>(OnTouchChanged);

			for (int i = 0; i < clickableObjects.Count; i++)
			{
				clickableObjects[i].gameObject.SetActive(false);
			}

			SetClickableObject(clickableObjects.FirstOrDefault());
			CurrentClickableObject.transform.position = settings.startPositions.RandomItem();
			yield return new WaitForSeconds(1f);
			TargetMoveIn();
		}

		private void OnDestroy()
		{
			signalBus?.Unsubscribe<SignalTouchChanged>(OnTouchChanged);
		}

		private void SetClickableObject(ClickableObject clickable)
		{
			var lastTarget = CurrentClickableObject;

			if (CurrentClickableObject != null)
			{
				CurrentClickableObject.onDead -= OnClickableObjectDead;
				CurrentClickableObject.gameObject.SetActive(false);
				CurrentClickableObject.transform.SetParent(clickableConveyor);
				CurrentClickableObject.ResetFlip();
			}

			CurrentClickableObject = clickable;
			Assert.IsTrue(CurrentClickableObject != null);
			CurrentClickableObject.gameObject.SetActive(true);
			signalBus?.Fire(new SignalTargetChanged() { clickable = CurrentClickableObject });

			if (CurrentClickableObject != null)
			{
				currentIndex = clickableObjects.IndexOf(CurrentClickableObject);
				CurrentClickableObject.onDead += OnClickableObjectDead;
				CurrentClickableObject.transform.SetParent(clickableContent);
			}

			if(lastTarget != null)
			{
				lastTarget.Sheet.Refresh();
			}
		}

		[Button]
		private void Next()
		{
			gameManager.ChangeState(GameState.None);

			DG.Tweening.Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CurrentClickableObject.Flip())
				.Append(CurrentClickableObject.transform.DOMove(Vector3.left * 5f, 0.2f))
				.OnComplete(() =>
				{
					currentIndex = (currentIndex + 1) % clickableObjects.Count;
					SetClickableObject(clickableObjects[currentIndex]);
					CurrentClickableObject.transform.position = settings.startPositions.RandomItem();
					TargetMoveIn();
				});
		}

		private void TargetMoveIn()
		{
			CurrentClickableObject.transform
				.DOMove(Vector3.zero, 0.35f)
				.SetEase(Ease.OutBounce)
				.OnComplete(() =>
				{
					gameManager.ChangeState(GameState.Gameplay);
				});
		}

		private void OnClickableObjectDead()
		{
			Next();
		}

		private void OnTouchChanged(SignalTouchChanged signal)
		{
			if (gameManager.CurrentGameState != GameState.Gameplay) return;

			Touch touch = signal.touch;

			if (player.TapCount.CurrentValue % 2 == 0)
			{
				if (touch.phase == TouchPhase.Began)
				{
					clickerLeftHand.Punch();
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					clickerLeftHand.Back();
					clickerRightHand.Back();
				}
			}
			else
			{
				if (touch.phase == TouchPhase.Began)
				{
					clickerRightHand.Punch();
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					clickerLeftHand.Back();
					clickerRightHand.Back();
				}
			}
		}

#if UNITY_EDITOR
		[Button(DirtyOnClick = true)]
		private void Refresh()
		{
			if (settings.nextEnemies.Count == 0 || clickableContent == null) return;

			clickableObjects.Clear();
			clickableContent.DestroyChildren(true);
			clickableConveyor.DestroyChildren(true);
			for (int i = 0; i < settings.nextEnemies.Count; i++)
			{
				var clickable = Create(settings.nextEnemies[i].prefab);
				clickable.transform.position = new Vector3(settings.spacing, 0, 0) * i;
				clickable.Collider.enabled = i == 0;

				clickableObjects.Add(clickable);
			}

			ClickableObject Create(ClickableObject prefab)
			{
				var obj = PrefabUtility.InstantiatePrefab(prefab);
				var clickable = obj.GetComponent<ClickableObject>();
				clickable.transform.SetParent(clickableConveyor);
				clickable.transform.localScale = Vector3.one;

				return clickable;
			}
		}

		private void OnDrawGizmos()
		{
			for (int i = 0; i < settings.startPositions.Count; i++)
			{
				Gizmos.DrawSphere(settings.startPositions[i], 0.1f);
				Handles.Label(settings.startPositions[i], $"p{i}");
			}
		}
#endif
	}

	[System.Serializable]
	public class ConveyorSettings
	{
		public List<EnemyData> nextEnemies = new List<EnemyData>();

		[Min(0)]
		public float spacing = 1f;
		public List<Vector3> startPositions = new List<Vector3>();
	}
}