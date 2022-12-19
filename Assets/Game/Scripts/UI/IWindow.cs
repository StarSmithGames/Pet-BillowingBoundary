using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.UI
{
	public interface IWindow
    {
		bool IsShowing { get; }
		bool IsInProcess { get; }

		void Enable(bool trigger);
		void Show(UnityAction callback = null);
		void Hide(UnityAction callback = null);
	}

	public abstract class WindowBase : MonoBehaviour, IWindow
	{
		public bool IsShowing { get; protected set; }
		public bool IsInProcess { get; protected set; }

		[field: SerializeField] public CanvasGroup CanvasGroup { get; protected set; }

		public virtual void Show(UnityAction callback = null)
		{
			IsInProcess = true;
			CanvasGroup.alpha = 0f;
			CanvasGroup.Enable(true, false);
			IsShowing = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(1f, 0.2f))
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
				.Append(CanvasGroup.DOFade(0f, 0.15f))
				.AppendCallback(() =>
				{
					CanvasGroup.Enable(false);
					IsShowing = false;
					callback?.Invoke();

					IsInProcess = false;
				});
		}

		public virtual void Enable(bool trigger)
		{
			CanvasGroup.Enable(trigger);
			IsShowing = trigger;
		}

		[Button(DirtyOnClick = true)]
		private void OpenClose()
		{
			Enable(CanvasGroup.alpha == 0f ? true : false);
		}
	}

	public abstract class WindowBasePoolable : WindowBase, IPoolable
	{
		public IMemoryPool Pool { get => pool; protected set => pool = value; }
		private IMemoryPool pool;

		public virtual void Hide(bool despawnIt = true, UnityAction callback = null)
		{
			Hide(() =>
			{
				callback?.Invoke();
				if (despawnIt)
				{
					DespawnIt();
				}
			});
		}

		public void DespawnIt()
		{
			pool?.Despawn(this);
		}

		public virtual void OnSpawned(IMemoryPool pool)
		{
			this.pool = pool;
		}

		public virtual void OnDespawned()
		{
			pool = null;
		}
	}
}