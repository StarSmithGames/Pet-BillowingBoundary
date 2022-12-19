using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.AchievementSystem
{
	public class UIAchievementItem : PoolableObject
	{
		//[field: SerializeField] public Image Bar { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI AchievementName { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI AchievementRule { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI AchievementCount { get; private set; }
	}
}