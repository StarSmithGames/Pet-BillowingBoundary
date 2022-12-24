using DG.Tweening;

using Game.Entities;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using Zenject;

using Unity.VisualScripting;

using Game.Managers.GameManager;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Managers.ClickManager
{
	public class ClickerConveyor : MonoBehaviour
	{
		public Transform TargetContent => targetContent;
		public Transform ClickableConveyor => clickableConveyor;

		public ClickerHand CurrentLeftHand => leftHand;
		public ClickerHand CurrentRightHand => rightHand;

		[Header("Components")]
		[SerializeField] private Transform targetContent;
		[SerializeField] private Transform clickableConveyor;
		[Space]
		[SerializeField] private ClickerHand leftHand;
		[SerializeField] private ClickerHand rightHand;
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
		private void Construct(SignalBus signalBus,
			Player player,
			GameManager.GameManager gameManager)
		{
			this.signalBus = signalBus;
			this.player = player;
			this.gameManager = gameManager;
		}

		private void Start()
		{
			signalBus?.Subscribe<SignalTouchChanged>(OnTouchChanged);
		}

		private void OnDestroy()
		{
			signalBus?.Unsubscribe<SignalTouchChanged>(OnTouchChanged);
		}

		public ClickableObject CreateTarget()
		{
			currentIndex = (currentIndex + 1) % clickableObjects.Count;
			return clickableObjects[currentIndex];
		}

		public Vector3 GetRandomStartPosition()
		{
			return settings.startPositions.RandomItem();
		}

		private void OnTouchChanged(SignalTouchChanged signal)
		{
			if (gameManager.CurrentGameState != GameState.Gameplay) return;

			Touch touch = signal.touch;

			if (player.Taps.CurrentValue % 2 == 0)
			{
				if (touch.phase == TouchPhase.Began)
				{
					leftHand.Punch();
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					leftHand.Back();
					rightHand.Back();
				}
			}
			else
			{
				if (touch.phase == TouchPhase.Began)
				{
					rightHand.Punch();
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					leftHand.Back();
					rightHand.Back();
				}
			}
		}

#if UNITY_EDITOR
		[Button(DirtyOnClick = true)]
		private void Refresh()
		{
			if (settings.nextEnemies.Count == 0 || targetContent == null) return;

			clickableObjects.Clear();
			targetContent.DestroyChildren(true);
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