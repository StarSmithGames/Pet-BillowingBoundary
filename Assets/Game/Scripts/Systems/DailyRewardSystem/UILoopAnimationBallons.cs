using DG.Tweening;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.DailyRewardSystem
{
	public class UILoopAnimationBallons : MonoBehaviour
	{
		private void Start()
		{
			StartCoroutine(Rotate());
		}

		private IEnumerator Rotate()
		{
			var euler = transform.eulerAngles;

			float t = Random.Range(0.8f, 2f);

			while (true)
			{
				yield return transform.DORotate(new Vector3(euler.x, euler.y, -25f), t).WaitForCompletion();
				yield return transform.DORotate(new Vector3(euler.x, euler.y, 25f), t).WaitForCompletion();
			}

		}
	}
}