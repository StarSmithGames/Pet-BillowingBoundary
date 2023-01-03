using DG.Tweening;

using Game.UI;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.FloatingSystem
{
	public class Showable3D : MonoBehaviour, IShowable
	{
		public bool IsShowing { get; protected set; }
		public bool IsInProcess { get; protected set; }

		[SerializeField] protected Renderer renderer;

		protected Material material;

		public virtual void Enable(bool trigger)
		{
			gameObject.SetActive(trigger);
		}

		public virtual void Show(UnityAction callback = null)
		{
			transform.localScale = Vector3.zero;
			Enable(true);
			IsInProcess = true;
			IsShowing = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(transform.DOScale(1f, 0.2f))
				.AppendCallback(() =>
				{
					callback?.Invoke();
					IsInProcess = false;
				});
		}

		public virtual void Hide(UnityAction callback = null)
		{
			IsInProcess = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(transform.DOScale(0f, 0.15f))
				.AppendCallback(() =>
				{
					IsShowing = false;
					IsInProcess = false;
					callback?.Invoke();
					Enable(false);
				});
		}
	}

	public class FloatingModel : Showable3D
	{
		public bool Is3D => is3D;
		[SerializeField] private bool is3D = true;
		[ShowIf("is3D")]
		[SerializeField] private Model3DSettings settings3D;

		public override void Enable(bool trigger)
		{
			if (trigger == false)
			{
				transform.DOKill(true);
			}
			base.Enable(trigger);
		}

		public void StartRotate()
		{
			transform
				.DORotate(settings3D.rotation, settings3D.duration, RotateMode.FastBeyond360)
				.SetLoops(-1, LoopType.Restart)
				.SetRelative()
				.SetEase(Ease.Linear);
		}

		public void Kill()
		{
			transform.DOKill(true);
		}
	}

	[System.Serializable]
	public class Model3DSettings
	{
		public float duration;
		public Vector3 rotation = new Vector3(0, 360f, 0);
	}
}