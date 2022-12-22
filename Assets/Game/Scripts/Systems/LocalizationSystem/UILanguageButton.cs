using Game.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.LocalizationSystem
{
	public class UILanguageButton : UIButtonToggle
	{
		public UnityAction<UILanguageButton> onClicked;

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

		protected override void OnClick()
		{
			if (IsEnable) return;

			base.OnClick();

			onClicked?.Invoke(this);
		}
	}
}