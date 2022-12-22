using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.MarketSystem
{
	public class UIIcon : MonoBehaviour
	{
		[field: SerializeField] public Image Icon { get; private set; }
		[field: SerializeField] public Image Shadow { get; private set; }
	}
}