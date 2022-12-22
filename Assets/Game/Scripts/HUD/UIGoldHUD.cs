using UnityEngine;

namespace Game.HUD
{
	public class UIGoldHUD : UIGold
	{
		public static UIGoldHUD Instance
		{
			get
			{
				if (instance == null)
				{
					instance = GameObject.FindObjectOfType<UIGoldHUD>();
				}

				return instance;
			}
		}
		private static UIGoldHUD instance;
	}
}