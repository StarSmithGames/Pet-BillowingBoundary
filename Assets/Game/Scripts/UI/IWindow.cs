using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.UI
{
	public interface IShowable
	{
		bool IsShowing { get; }
		bool IsInProcess { get; }

		void Enable(bool trigger);
		void Show(UnityAction callback = null);
		void Hide(UnityAction callback = null);
	}

	public interface IWindow : IShowable { }

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

	public abstract class WindowPopupBase : WindowBase
	{
		[field: SerializeField] public Transform Window { get; private set; }

		public override void Show(UnityAction callback = null)
		{
			Window.localScale = Vector3.zero;

			IsInProcess = true;
			CanvasGroup.alpha = 0f;
			CanvasGroup.Enable(true, false);
			IsShowing = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(1f, 0.2f))
				.Join(Window.DOScale(1, 0.35f).SetEase(Ease.OutBounce))
				.AppendCallback(() =>
				{
					callback?.Invoke();
					IsInProcess = false;
				});
		}

		public override void Hide(UnityAction callback = null)
		{
			Window.localScale = Vector3.one;

			IsInProcess = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(Window.DOScale(0, 0.25f).SetEase(Ease.InBounce))
				.Join(CanvasGroup.DOFade(0f, 0.25f))
				.AppendCallback(() =>
				{
					CanvasGroup.Enable(false);
					IsShowing = false;
					callback?.Invoke();

					IsInProcess = false;
				});
		}
	}

	public abstract class WindowQuartBase : WindowPopupBase
	{
		public override void Show(UnityAction callback = null)
		{
			Window.localScale = Vector3.zero;

			IsInProcess = true;
			CanvasGroup.alpha = 0f;
			CanvasGroup.Enable(true, false);
			IsShowing = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(1f, 0.2f))
				.Join(Window.DOScale(1, 0.35f).SetEase(Ease.OutQuart))
				.AppendCallback(() =>
				{
					callback?.Invoke();
					IsInProcess = false;
				});
		}

		public override void Hide(UnityAction callback = null)
		{
			Window.localScale = Vector3.one;

			IsInProcess = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(0f, 0.15f))
				.Join(Window.DOScale(0, 0.25f).SetEase(Ease.InBounce))
				.AppendCallback(() =>
				{
					CanvasGroup.Enable(false);
					IsShowing = false;
					callback?.Invoke();

					IsInProcess = false;
				});
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

	public abstract class WindowPopupBasePoolable : WindowPopupBase, IPoolable
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