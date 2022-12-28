using DG.Tweening;
using Game.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Game.HUD
{
	public class UIAlert : WindowPopupBase
	{
		private void Start()
		{
			Enable(false);	
		}
		
		public override void Show(UnityAction callback = null)
		{
			base.Show(() =>
			{
				callback?.Invoke();
				IdleAnimation().Play();
			});
		}
		
		public override void Hide(UnityAction callback = null)
		{
			transform.DOKill(true);

			base.Hide(callback);
		}

		private Tween IdleAnimation()
		{
			return transform
				.DOPunchScale(new Vector3(0.25f, 0.25f, 0.25f), 0.2f)
				.SetLoops(-1)
				.SetEase(Ease.OutBounce);
		}
	}
}