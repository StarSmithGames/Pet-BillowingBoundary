using DG.Tweening;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

namespace Game.Systems.FloatingTextSystem
{
	public class FloatingTextSystem
	{
		private FloatingText.Factory floatingTextFactory;
		private Camera camera;

		private FloatingTextSystem(FloatingText.Factory floatingTextFactory)
		{
			this.floatingTextFactory = floatingTextFactory;
			camera = Camera.main;
		}

		public void CreateText(Vector3 position, string text, Color? color = null, AnimationType type = AnimationType.BasicDamage)
		{
			switch (type)
			{
				case AnimationType.BasicDamage:
				{
					var obj = Create();

					Sequence sequence = DOTween.Sequence();

					position.y += 2;

					obj.transform.DOMove(position, 0.5f).SetEase(Ease.OutQuint);
					sequence
						.Append(obj.Text.DOFade(0f, 0.7f))
						.AppendCallback(obj.DespawnIt);
					break;
				}

				case AnimationType.AdvanceDamage:
				{
					var obj = Create();

					obj.transform.localScale = Vector3.zero;

					float offsetX = Random.Range(-1.5f, 1.5f);
					if (Mathf.Abs(offsetX) <= 0.2f) offsetX += Mathf.Sign(offsetX) * 0.1f;

					obj.transform.position += Vector3.up;

					Vector3 endPosition = obj.transform.position + (Vector3.right * offsetX) + (Vector3.up * Random.Range(0f, 1f));

					Sequence sequence = DOTween.Sequence();

					sequence
						.Append(obj.transform.DOJump(endPosition, Random.Range(0.5f, 1.5f), 1, 1f))
						.Join(obj.transform.DOScale(1f, 0.5f))
						.Append(obj.Text.DOFade(0f, 0.5f))
						.AppendCallback(obj.DespawnIt);
					break;
				}


				default:
				{
					var obj = Create();

					Sequence sequence = DOTween.Sequence();
					sequence
						.AppendInterval(1f)
						.AppendCallback(obj.DespawnIt);
					break;
				}
			}


			FloatingText Create()
			{
				var item = floatingTextFactory.Create();

				item.Text.color = color ?? Color.white;
				item.Text.text = text;
				item.transform.position = position;
				item.transform.rotation = camera.transform.rotation;//billboard, add to update?

				return item;
			}
		}
	}

	public enum AnimationType
	{
		None,
		BasicDamage,
		AdvanceDamage,
	}
}