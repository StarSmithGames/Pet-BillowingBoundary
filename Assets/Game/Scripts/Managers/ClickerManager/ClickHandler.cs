using Game.Entities;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Zenject;

namespace Game.Managers.ClickManager
{
	public class ClickHandler : ITickable
	{
		public UnityAction onTouchBegan;
		public UnityAction onTouchEnded;

		private int maxTapCount = 0;

		private SignalBus signalBus;
		private Player player;

		public ClickHandler(SignalBus signalBus, Player player)
		{
			this.signalBus = signalBus;
			this.player = player;
		}

		public void Tick()
		{
			if (EventSystem.current.IsPointerOverGameObject())//Windows
			{
				return;
			}

			if (Input.touchCount > 0)
			{
				for (int i = 0; i < Input.touchCount; i++)
				{
					var touch = Input.GetTouch(i);
					int id = touch.fingerId;
					if (EventSystem.current.IsPointerOverGameObject(id))//Mobile
					{
						return;
					}

					if (touch.phase == TouchPhase.Began)
					{
						player.TapCount.CurrentValue++;

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

					if (touch.tapCount > maxTapCount)
					{
						maxTapCount = touch.tapCount;
					}
				}
			}
		}
	}
}
//Debug.LogError($"Touch {i} -Position {touch.position} -Tap Count: {touch.tapCount} -Finger ID: {touch.fingerId}\nRadius: {touch.radius} ({((touch.radius / (touch.radius + touch.radiusVariance)) * 100f)}%)");