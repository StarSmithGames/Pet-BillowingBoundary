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
using Game.Systems.WaveRoadSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Managers.ClickManager
{
	public class ClickerConveyor : MonoBehaviour
	{
		public Transform TargetContent => targetContent;

		public ClickerHand CurrentLeftHand => leftHand;
		public ClickerHand CurrentRightHand => rightHand;

		[Header("Components")]
		[SerializeField] private Transform targetContent;
		[SerializeField] private Transform clickableConveyor;
		[Space]
		[SerializeField] private ClickerHand leftHand;
		[SerializeField] private ClickerHand rightHand;
		[Header("Vars")]
		[SerializeField] private ConveyorSettings settings;

		private int currentIndex = -1;

		private SignalBus signalBus;
		private Player player;
		private WaveRoad waveRoad;
		private GameManager.GameManager gameManager;

		[Inject]
		private void Construct(SignalBus signalBus,
			Player player,
			WaveRoad waveRoad,
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
		//[Button(DirtyOnClick = true)]
		//private void Refresh()
		//{
		//	if (settings.nextEnemies.Count == 0 || targetContent == null) return;

		//	clickableObjects.Clear();
		//	targetContent.DestroyChildren(true);
		//	clickableConveyor.DestroyChildren(true);
		//	for (int i = 0; i < settings.nextEnemies.Count; i++)
		//	{
		//		var clickable = Create(settings.nextEnemies[i].prefab);
		//		clickable.transform.position = new Vector3(settings.spacing, 0, 0) * i;
		//		clickable.Collider.enabled = i == 0;

		//		clickableObjects.Add(clickable);
		//	}

		//	ClickableObject Create(ClickableObject prefab)
		//	{
		//		var obj = PrefabUtility.InstantiatePrefab(prefab);
		//		var clickable = obj.GetComponent<ClickableObject>();
		//		clickable.transform.SetParent(clickableConveyor);
		//		clickable.transform.localScale = Vector3.one;

		//		return clickable;
		//	}
		//}

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
		public List<Vector3> startPositions = new List<Vector3>();
	}
}