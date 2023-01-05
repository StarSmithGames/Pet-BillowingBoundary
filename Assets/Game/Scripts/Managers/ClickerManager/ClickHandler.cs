using Game.Entities;
using Game.Managers.VibrationManager;
using Game.Systems.AchievementSystem;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Zenject;

namespace Game.Managers.ClickManager
{
	public class ClickHandler : IInitializable, ITickable
	{
		public UnityAction onTouchBegan;
		public UnityAction onTouchEnded;

		private int maxTapCount = 0;
		private int tapsInSecond = 0;
		private int criticalTapsInSecond = 0;
		private float t = 0;

		private Taps taps;
		private Taps criticalTaps;

		private SignalBus signalBus;
		private Player player;
		private GameManager.GameManager gameManager;
		private AchievementSystem achievementSystem;

		public ClickHandler(SignalBus signalBus,
			Player player,
			GameManager.GameManager gameManager,
			AchievementSystem achievementSystem)
		{
			this.signalBus = signalBus;
			this.player = player;
			this.gameManager = gameManager;
			this.achievementSystem = achievementSystem;
		}

		public void Initialize()
		{
			taps = player.Taps;
			criticalTaps = player.CriticalTaps;

			criticalTaps.onChanged += OnCriticalTapsChanged;
		}

		public void Tick()
		{
			if (gameManager.CurrentGameState != GameManager.GameState.Gameplay) return;

			if (EventSystem.current.IsPointerOverGameObject())//Windows
			{
				return;
			}
			if (IsPointerOverUIObject())//Mobile
			{
				return;
			}

			if (Input.touchCount > 0)
			{
				for (int i = 0; i < Input.touchCount; i++)
				{
					var touch = Input.GetTouch(i);

					if (touch.phase == TouchPhase.Began)
					{
						taps.CurrentValue++;
						tapsInSecond++;

						signalBus.Fire(new SignalTouchChanged()
						{
							touch = touch,
						});
					}

					if (touch.phase == TouchPhase.Ended)
					{
						signalBus.Fire(new SignalTouchChanged()
						{
							touch = touch,
						});
					}

					//if (touch.tapCount > maxTapCount)
					//{
					//	maxTapCount = touch.tapCount;
					//}
				}
			}

			t += Time.deltaTime;

			if (t >= 1f)
			{
				if(tapsInSecond >= 12)
				{
					achievementSystem.Achive_achievement_rapid_fire();
				}

				if(criticalTapsInSecond >= 2)
				{
					achievementSystem.Achive_achievement_monster();
				}

				t = 0;
				tapsInSecond = 0;
				criticalTapsInSecond = 0;
			}
		}

		private bool IsPointerOverUIObject()
		{
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			return results.Count > 0;
		}

		private void OnCriticalTapsChanged()
		{
			criticalTapsInSecond++;
		}
	}
}
//Debug.LogError($"Touch {i} -Position {touch.position} -Tap Count: {touch.tapCount} -Finger ID: {touch.fingerId}\nRadius: {touch.radius} ({((touch.radius / (touch.radius + touch.radiusVariance)) * 100f)}%)");