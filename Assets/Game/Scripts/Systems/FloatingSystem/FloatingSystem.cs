using DG.Tweening;

using System.Linq;

using UnityEngine;

namespace Game.Systems.FloatingSystem
{
	public class FloatingSystem
	{
		private CameraSystem.CameraSystem cameraSystem;
		private FloatingText.Factory floatingTextFactory;
		private FloatingTextUI.Factory floatingTextUIFactory;
		private FloatingCoin2D.Factory floatingCoin2DFactory;
		private FloatingCoin3D.Factory floatingCoin3DFactory;

		private FloatingSystem(CameraSystem.CameraSystem cameraSystem,
			FloatingText.Factory floatingTextFactory,
			FloatingTextUI.Factory floatingTextUIFactory,
			FloatingCoin2D.Factory floatingCoin2DFactory,
			FloatingCoin3D.Factory floatingCoin3DFactory)
		{
			this.cameraSystem = cameraSystem;
			this.floatingTextFactory = floatingTextFactory;
			this.floatingTextUIFactory = floatingTextUIFactory;
			this.floatingCoin2DFactory = floatingCoin2DFactory;
			this.floatingCoin3DFactory = floatingCoin3DFactory;
		}

		public void CreateText(Vector3 position, string text, Color? color = null, AnimationType type = AnimationType.BasicDamage)
		{
			var obj = Create();

			ApplyAnimation(obj, type);

			FloatingText Create()
			{
				var item = floatingTextFactory.Create();

				item.Text.color = color ?? Color.white;
				item.Text.text = text;
				item.transform.position = position;
				item.transform.rotation = cameraSystem.transform.rotation;//billboard, add to update?

				return item;
			}
		}

		public void CreateTextUI(Vector3 position, string text, Color? color = null, AnimationType type = AnimationType.BasicDamage)
		{
			var obj = Create();

			ApplyAnimation(obj, type);

			FloatingTextUI Create()
			{
				var item = floatingTextUIFactory.Create();

				item.Text.color = color ?? Color.white;
				item.Text.text = text;
				item.transform.position = position;
				item.transform.rotation = cameraSystem.transform.rotation;//billboard, add to update?

				return item;
			}
		}

		public void CreateCoin3D(Vector3 position)
		{
			Sequence sequence = DOTween.Sequence();

			var obj = Create();

			sequence
				.AppendCallback(() => obj.Rigidbody.AddForce(new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(0.2f, 0.7f), 0) * Random.Range(300f, 800f)))
				.AppendInterval(0.5f)
				.Append(obj.Fade(0, 0.25f))
				.OnComplete(obj.DespawnIt);

			FloatingCoin3D Create()
			{
				var item = floatingCoin3DFactory.Create();
				item.SetFade(1f);
				item.transform.position = position;
				item.StartRotate();

				return item;
			}
		}

		public void CreateCoin2D(Vector3 position, Transform target)
		{
			Sequence sequence = DOTween.Sequence();

			var obj = Create();

			sequence
				.Append(obj.transform.DOMove(target.position, Random.Range(0.15f, 0.25f)))
				.Append(obj.Fade(0, 0.1f))
				.OnComplete(obj.DespawnIt);

			FloatingCoin2D Create()
			{
				var item = floatingCoin2DFactory.Create();
				item.SetFade(1f);
				item.transform.position = position;

				return item;
			}
		}

		private void ApplyAnimation(FloatingObject floatingObject, AnimationType type)
		{
			Vector3 position = floatingObject.transform.position;

			switch (type)
			{
				case AnimationType.BasicDamage:
				{
					Sequence sequence = DOTween.Sequence();
					position.y += 2;

					sequence
						.Append(floatingObject.transform.DOMove(position, 0.5f).SetEase(Ease.OutQuint))
						.Append(floatingObject.Fade(0f, 0.7f))
						.AppendCallback(floatingObject.DespawnIt);
					break;
				}
				case AnimationType.AdvanceDamage:
				{
					floatingObject.transform.localScale = Vector3.zero;

					float offsetX = Random.Range(-1.5f, 1.5f);
					if (Mathf.Abs(offsetX) <= 0.2f) offsetX += Mathf.Sign(offsetX) * 0.1f;

					floatingObject.transform.position += Vector3.up;

					Vector3 endPosition = floatingObject.transform.position + (Vector3.right * offsetX) + (Vector3.up * Random.Range(0f, 1f));

					Sequence sequence = DOTween.Sequence();

					sequence
						.Append(floatingObject.transform.DOJump(endPosition, Random.Range(0.5f, 1.5f), 1, 1f))
						.Join(floatingObject.transform.DOScale(1f, 0.5f))
						.Append(floatingObject.Fade(0, 0.5f))
						.AppendCallback(floatingObject.DespawnIt);
					break;
				}
				case AnimationType.AddGold:
				{
					Sequence sequence = DOTween.Sequence();
					position.y -= 100;
					sequence
						.Append(floatingObject.transform.DOMove(position, 0.5f).SetEase(Ease.OutQuint))
						.Append(floatingObject.Fade(0f, 0.7f))
						.AppendCallback(floatingObject.DespawnIt);
					break;
				}
				default:
				{
					Sequence sequence = DOTween.Sequence();
					sequence
						.AppendInterval(1f)
						.AppendCallback(floatingObject.DespawnIt);
					break;
				}
			}
		}
	}

	public enum AnimationType
	{
		None,
		BasicDamage,
		AdvanceDamage,
		AddGold,
	}
}