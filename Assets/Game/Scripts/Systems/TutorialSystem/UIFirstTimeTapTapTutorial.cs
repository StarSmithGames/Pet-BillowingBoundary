using Game.Entities;
using Game.Managers.StorageManager;
using Game.UI;

using System.Collections;
using System.Collections.Generic;
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

		private void Start()
		{
			Enable(false);

			if(saveLoad.GetStorage().IsCompleteTutorial.GetData() == false)
			{
				player.onTapChanged += OnTapChanged;
				Show();
			}
		}


		private void OnTapChanged()
		{
			player.onTapChanged -= OnTapChanged;
			Hide();
		}
	}
}