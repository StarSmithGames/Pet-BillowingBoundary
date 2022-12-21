using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIBarColors : MonoBehaviour
    {
		[OnValueChanged("OnColorsChanged", true)]
		[SerializeField] private BarColors barColors;

		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			barColors.borderDark = transform.GetChildComponentByName<Image>("Border").color;
			barColors.backgroundNormal = transform.GetChildComponentByName<Image>("Background").color;
			barColors.gradientHightlight = transform.GetChildComponentByName<Image>("Gradient").color;
		}
		private void OnColorsChanged()
		{
			transform.GetChildComponentByName<Image>("Border").color = barColors.borderDark;
			transform.GetChildComponentByName<Image>("Background").color = barColors.backgroundNormal;
			transform.GetChildComponentByName<Image>("Gradient").color = barColors.gradientHightlight;
		}
	}

	[System.Serializable]
	public class BarColors
	{
		public Color borderDark;
		public Color backgroundNormal;
		public Color gradientHightlight;
	}
}