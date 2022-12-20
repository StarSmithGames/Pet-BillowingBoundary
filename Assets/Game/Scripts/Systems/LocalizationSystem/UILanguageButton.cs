using Game.UI;
using UnityEngine;

namespace Game.Systems.LocalizationSystem
{
	public class UILanguageButton : UIButtonToggle
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Text { get; private set; }
	}
}