using Game.UI;
using UnityEngine;

namespace Game.Systems.LocalizationSystem
{
	public class UILanguageButton : UIButtonToggle
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }

		public void SetText(string text)
		{
			if (text.IsEmpty())
			{
				Text.text = "NULL";
				return;
			}

			if (text.Contains("("))
			{
				text = text.Split(' ')[0];
			}

			Text.text = text.FirstCharToUpper();
		}
	}
}