using Game.Entities;
using Game.Systems.PremiumMarketSystem;
using Game.UI;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.HUD
{
	public class UIGold : MonoBehaviour
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Count { get; private set; }
		[field: SerializeField] public Button Button { get; private set; }

		private UISubCanvas subCanvas;
		private Gold gold;

		[Inject]
		private void Construct(UISubCanvas subCanvas, Player player)
		{
			this.subCanvas = subCanvas;
			this.gold = player.Gold;
		}

		private void Start()
		{
			Button.onClick.AddListener(OnClick);

			gold.onChanged += OnTapCountChanged;
			Count.text = gold.Output;
		}

		private void OnDestroy()
		{
			Button?.onClick.RemoveAllListeners();

			if (gold != null)
			{
				gold.onChanged -= OnTapCountChanged;
			}
		}

		private void OnTapCountChanged()
		{
			Count.text = gold.Output;
		}

		private void OnClick()
		{
			subCanvas.WindowsRegistrator.Show<PremiumMarketWindow>();
		}
	}
}