using Game.Entities;
using Game.HUD;
using Game.Managers.AsyncManager;

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

		public FloatingAwards(FloatingSystem floatingSystem, AsyncManager asyncManager, Player player)
		{
			this.floatingSystem = floatingSystem;
			this.asyncManager = asyncManager;
			this.player = player;
		}

		///Count min 5
		///Time required 0.05 * count
		public Coroutine StartAwardCoins(Vector3 startPosition, int count, BFN addCoins, UnityAction callback = null)
		{
			return asyncManager.StartCoroutine(Test(startPosition, UIGoldHUD.Instance.transform, count, addCoins, callback));
		}

		
		private IEnumerator Test(Vector3 startPosition, Transform target, int count, BFN addCoins, UnityAction callback)
		{
			var wait = new WaitForSeconds(0.025f);

			for (int i = 0; i < 5; i++)
			{
				floatingSystem.CreateCoin2D(startPosition, target);

				yield return wait;
			}

			count = count - 5;

			asyncManager.StartCoroutine(LerpGoldTo(player.Gold.CurrentValue + addCoins, count * 0.05f));

			for (int i = 0; i < count; i++)
			{
				floatingSystem.CreateCoin2D(startPosition, target);

				yield return wait;
			}

			callback?.Invoke();
		}

		private IEnumerator LerpGoldTo(BFN end, float duration)
		{
			var start = player.Gold.CurrentValue;

			float t = 0;

			while (t < duration)
			{
				player.Gold.CurrentValue = BFN.Lerp(start, end, t);

				t += Time.deltaTime;

				yield return null;
			}
		}
	}
}