using Game.Entities;
using Game.HUD;
using Game.Managers.AsyncManager;
using Game.Managers.AudioManager;

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.FloatingSystem
{
	public class FloatingAwards
	{
		private FloatingSystem floatingSystem;
		private AsyncManager asyncManager;
		private Player player;
		private AudioManager audioManager;

		public FloatingAwards(FloatingSystem floatingSystem, AsyncManager asyncManager, Player player, AudioManager audioManager)
		{
			this.floatingSystem = floatingSystem;
			this.asyncManager = asyncManager;
			this.player = player;
			this.audioManager = audioManager;
		}

		///Time required 0.05 * count
		public Coroutine StartAwardCoins(Vector3 startPosition, int count, BFN addCoins, UnityAction callback = null)
		{
			return asyncManager.StartCoroutine(Test(startPosition, UIGoldHUD.Instance.transform, count, addCoins, callback));
		}

		
		private IEnumerator Test(Vector3 startPosition, Transform target, int count, BFN addCoins, UnityAction callback)
		{
			audioManager.PlayCoinsReward();

			asyncManager.StartCoroutine(LerpGoldTo(player.Gold.CurrentValue + addCoins, count * 0.05f));
			var wait = new WaitForSeconds(0.05f);
			for (int i = 0; i < count; i++)
			{
				floatingSystem.CreateCoin2D(startPosition, target);

				yield return wait;
			}

			callback?.Invoke();
		}

		private IEnumerator LerpGoldTo(BFN end, float duration)
		{
			yield return new WaitForSeconds(Random.Range(0.15f, 0.25f));//Wait first floatingSystem.CreateCoin2D

			var start = player.Gold.CurrentValue;

			float t = 0;

			while (t < duration)
			{
				player.Gold.CurrentValue = BFN.Lerp(start, end, t);

				t += Time.deltaTime;

				UIGoldHUD.Instance.Punch();

				yield return null;
			}
		}
	}
}