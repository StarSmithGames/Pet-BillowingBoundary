using Game.Entities;
using Game.Systems.PremiumMarketSystem;
using Game.UI;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.HUD
{
	public class UIGold : UIButton
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Count { get; private set; }

		private UISubCanvas subCanvas;
		private Gold gold;

		[Inject]
		private void Construct(UISubCanvas subCanvas, Player player)
		{
			this.subCanvas = subCanvas;
			this.gold = player.Gold;
		}

		protected override void Start()
		{
			base.Start();
			gold.onChanged += OnTapCountChanged;
			Count.text = gold.Output;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (gold != null)
			{
				gold.onChanged -= OnTapCountChanged;
			}
		}

		private void OnTapCountChanged()
		{
			Count.text = gold.Output;
		}

		protected override void OnClick()
		{
			subCanvas.WindowsRegistrator.Show<PremiumMarketWindow>();

			base.OnClick();
		}
	}
}