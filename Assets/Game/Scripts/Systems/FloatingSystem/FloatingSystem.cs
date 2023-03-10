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
		private FloatingCandy3D.Factory floatingCandy3DFactory;

		private FloatingSystem(CameraSystem.CameraSystem cameraSystem,
			FloatingText.Factory floatingTextFactory,
			FloatingTextUI.Factory floatingTextUIFactory,
			FloatingCoin2D.Factory floatingCoin2DFactory,
			FloatingCoin3D.Factory floatingCoin3DFactory,
			FloatingCandy3D.Factory floatingCandy3DFactory)
		{
			this.cameraSystem = cameraSystem;
			this.floatingTextFactory = floatingTextFactory;
			this.floatingTextUIFactory = floatingTextUIFactory;
			this.floatingCoin2DFactory = floatingCoin2DFactory;
			this.floatingCoin3DFactory = floatingCoin3DFactory;
			this.floatingCandy3DFactory = floatingCandy3DFactory;
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

		public void Create3D(Vector3 position, bool isCoin)
		{
			Sequence sequence = DOTween.Sequence();

			var obj = Create();

			Vector3 force = isCoin ?
				new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(0.2f, 0.7f), 0) * Random.Range(300f, 800f) :
				new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * Random.Range(300f, 800f);

			sequence
				.AppendCallback(() => obj.Rigidbody.AddForce(force))
				.AppendInterval(Random.Range(0.5f, 1f))
				.OnComplete(obj.DespawnIt);

			Floating3D Create()
			{
				Floating3D item3D = null;

				if (isCoin)
				{
					item3D = floatingCoin3DFactory.Create();
				}
				else
				{
					item3D = floatingCandy3DFactory.Create();
				}

				item3D.transform.position = position;

				return item3D;
			}
		}

		public void CreateCoin2D(Vector3 position, Transform target)
		{
			Sequence sequence = DOTween.Sequence();

			var obj = Create();

			var rnd = obj.transform.position + (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * Screen.width / 4);

			sequence
				.Append(obj.transform.DOPath(new Vector3[] { rnd, target.position }, Random.Range(0.6f, 1.1f), PathType.CatmullRom))
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
					position.y -= Random.Range(50, 125);
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