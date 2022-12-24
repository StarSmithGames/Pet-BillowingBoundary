using DG.Tweening;

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

		[SerializeField] private Transform puncher;
		[SerializeField] private PunchSettings punchSettings;

		public void Punch()
		{
			puncher.DORewind();
			puncher.DOPunchScale(punchSettings.GetPunch(), punchSettings.duration, punchSettings.vibrato, punchSettings.elasticity);
		}
	}
}