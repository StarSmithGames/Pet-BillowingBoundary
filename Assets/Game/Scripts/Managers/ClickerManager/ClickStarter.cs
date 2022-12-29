using Game.Entities;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Managers.GameManager;
using Game.Systems.WaveRoadSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Managers.ClickManager
{
	public class ClickStarter : MonoBehaviour
	{
		public Transform TargetContent => targetContent;

		public ClickerHand CurrentLeftHand => leftHand;
		public ClickerHand CurrentRightHand => rightHand;

		[Header("Components")]
		[SerializeField] private Transform targetContent;
		[Space]
		[SerializeField] private ClickerHand leftHand;
		[SerializeField] private ClickerHand rightHand;
		[Header("Vars")]
		[SerializeField] private List<Vector3> startPositions = new List<Vector3>();

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

		private void Awake()
		{
			TargetContent.DestroyChildren();
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
			return startPositions.RandomItem();
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
		private void OnDrawGizmos()
		{
			for (int i = 0; i < startPositions.Count; i++)
			{
				Gizmos.DrawSphere(startPositions[i], 0.1f);
				Handles.Label(startPositions[i], $"p{i}");
			}
		}
#endif
	}

}