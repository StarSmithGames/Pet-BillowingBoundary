using Game.Entities;
using Game.HUD;
using Game.Managers.AsyncManager;
using Game.Managers.AudioManager;
using Game.Managers.StorageManager;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.FloatingSystem
{
	public class FloatingAwards
	{
		private BFN targetGold = BFN.Zero;
		private BFN maxCoins = new BFN(10000, 0);
		private Coroutine lerpGoldCoroutine;

		private SignalBus signalBus;
		private FloatingSystem floatingSystem;
		private AsyncManager asyncManager;
		private Player player;
		private AudioManager audioManager;

		public FloatingAwards(SignalBus signalBus, FloatingSystem floatingSystem, AsyncManager asyncManager, Player player, AudioManager audioManager)
		{
			this.signalBus = signalBus;
			this.floatingSystem = floatingSystem;
			this.asyncManager = asyncManager;
			this.player = player;
			this.audioManager = audioManager;
		}

		public Coroutine StartAwardCoins(Vector3 startPosition, BFN addCoins, UnityAction callback = null)
		{
			return asyncManager.StartCoroutine(BurstCoins(startPosition, UIGoldHUD.Instance.transform, (int)Mathf.Lerp(10, 100, BFN.Percent(addCoins, maxCoins)), addCoins, callback));
		}

		//public Coroutine StartCollectCoins(Vector3 startPosition, BFN addCoins, UnityAction callback = null)
		//{
		//	return asyncManager.StartCoroutine(SlowCollectCoins(startPosition, UIGoldHUD.Instance.transform, (int)Mathf.Lerp(10, 100, BFN.Percent(addCoins, maxCoins)), addCoins, callback));
		//}

		private IEnumerator BurstCoins(Vector3 startPosition, Transform target, int count, BFN addCoins, UnityAction callback)
		{
			audioManager.PlayCoinsReward();

			StartLerpGold(count, addCoins);

			yield return null;
			for (int i = 0; i < count; i++)
			{
				floatingSystem.CreateCoin2D(startPosition, target);
			}


			callback?.Invoke();
		}

		//private IEnumerator SlowCollectCoins(Vector3 startPosition, Transform target, int count, BFN addCoins, UnityAction callback)
		//{
		//	audioManager.PlayCoinsReward();

		//	StartLerpGold(count, addCoins);

		//	var wait = new WaitForSeconds(0.05f);
		//	for (int i = 0; i < count; i++)
		//	{
		//		floatingSystem.CreateCoin2D(startPosition, target);

		//		yield return wait;
		//	}

		//	callback?.Invoke();
		//}

		private void StartLerpGold(int count, BFN addCoins)
		{
			if (lerpGoldCoroutine == null)
			{
				targetGold = player.Gold.CurrentValue + addCoins;
				lerpGoldCoroutine = asyncManager.StartCoroutine(LerpGoldTo(count * 0.05f));
			}
			else
			{
				targetGold += addCoins;
			}
		}

		private IEnumerator LerpGoldTo(float duration)
		{
			yield return new WaitForSeconds(Random.Range(0.15f, 0.25f));//Wait first floatingSystem.CreateCoin2D

			var start = player.Gold.CurrentValue;
			//var part = coins / duration;

			float t = 0;

			while (t < duration)
			{
				player.Gold.CurrentValue = BFN.Lerp(start, targetGold, t);

				t += Time.deltaTime;

				UIGoldHUD.Instance.Punch();

				yield return null;
			}

			lerpGoldCoroutine = null;

			signalBus?.Fire<SignalSave>();
		}
	}
}