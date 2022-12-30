using Game.Managers.AudioManager;
using Game.Managers.IAPManager;
using Game.Managers.VibrationManager;
using Game.UI;

using Sirenix.OdinInspector;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.PremiumMarketSystem
{
	public class PremiumMarketWindow : WindowPopupBase
	{
		[field: SerializeField] public Button Blank { get; private set; }
		[field: SerializeField] public Button Close { get; private set; }

		[SerializeField] private List<UIPremiumMarketItem> premiumItems = new List<UIPremiumMarketItem>();

		private UISubCanvas subCanvas;
		private IAPManager iapManager;
		private AudioManager audioManager;
		private VibrationManager vibrationManager;

		[Inject]
		private void Construct(UISubCanvas subCanvas, IAPManager iapManager, AudioManager audioManager, VibrationManager vibrationManager)
		{
			this.subCanvas = subCanvas;
			this.iapManager = iapManager;
			this.audioManager = audioManager;
			this.vibrationManager = vibrationManager;
		}

		private void Start()
		{
			Enable(false);

			Blank.onClick.AddListener(OnClosed);
			Close.onClick.AddListener(OnClosed);

			subCanvas.WindowsRegistrator.Registrate(this);

			Dictionary<string, PremiumItemData> items = new()
			{
				{ "free", new PremiumItemData(){ type =  PremiumItemType.ADS, baseCost = new BFN(1000, 0).compressed } },
				{ iapManager.item1, new PremiumItemData(){ type =  PremiumItemType.Simple, baseCost = new BFN(100000, 0).compressed } },
				{ iapManager.item2, new PremiumItemData(){ type =  PremiumItemType.Simple, baseCost = new BFN(10000000, 0).compressed } },
				{ iapManager.item3, new PremiumItemData(){ type =  PremiumItemType.Simple, baseCost = new BFN(1000000000, 0).compressed, baseAdd = new BFN(500000, 0).compressed } },
				{ iapManager.item4, new PremiumItemData(){ type =  PremiumItemType.Simple, baseCost = new BFN(100000000000, 0).compressed, baseAdd = new BFN(1000000, 0).compressed } },
				{ iapManager.freeMode, new PremiumItemData(){ type =  PremiumItemType.FreeMode } },
			};

			int i = 0;
			foreach (var item in items)
			{
				premiumItems[i].SetData(item.Key, item.Value);
				i++;
			}
		}

		private void OnDestroy()
		{
			Blank?.onClick.RemoveAllListeners();
			Close?.onClick.RemoveAllListeners();

			subCanvas.WindowsRegistrator.UnRegistrate(this);
		}

		public override void Show(UnityAction callback = null)
		{
			for (int i = 0; i < premiumItems.Count; i++)
			{
				premiumItems[i].UpdateUI();
			}

			base.Show(callback);
		}

		private void OnClosed()
		{
			Hide();

			audioManager.PlayButtonClick();
			vibrationManager.Vibrate();
		}

		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			premiumItems = GetComponentsInChildren<UIPremiumMarketItem>().ToList();
		}
	}

	public class PremiumItemData
	{
		public PremiumItemType type;
		public BFN baseCost;
		public BFN baseAdd;
	}
}