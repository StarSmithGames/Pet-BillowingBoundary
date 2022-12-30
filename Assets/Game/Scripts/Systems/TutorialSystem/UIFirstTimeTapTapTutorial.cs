using DG.Tweening;

using Game.Entities;
using Game.Managers.StorageManager;
using Game.UI;

using System.Collections;

using UnityEngine;
using Zenject;

namespace Game.Systems.TutorialSystem
{
	public class UIFirstTimeTapTapTutorial : WindowPopupBase
	{
		private ISaveLoad saveLoad;
		private Player player;

		[Inject]
		private void Construct(ISaveLoad saveLoad, Player player)
		{
			this.saveLoad = saveLoad;
			this.player = player;
		}

		private IEnumerator Start()
		{
			Enable(false);

			yield return new WaitForSeconds(1f);

			if (saveLoad.GetStorage().IsCompleteTutorial.GetData() == false)
			{
				player.Taps.onChanged += OnTapsChanged;
				Show(() =>
				{
					IdleAnimation().Play();
				});
			}
		}


		private void OnTapsChanged()
		{
			if (IsInProcess) return;

			player.Taps.onChanged -= OnTapsChanged;

			Hide(() =>
			{
				Window.DOKill(true);
			});

			saveLoad.GetStorage().IsCompleteTutorial.SetData(true);
		}

		private Tween IdleAnimation()
		{
			return Window
				.DOPunchScale(new Vector3(0.2f, 0.5f, 0.2f), 0.17f)
				.SetLoops(-1)
				.SetEase(Ease.OutBounce);
		}
	}
}