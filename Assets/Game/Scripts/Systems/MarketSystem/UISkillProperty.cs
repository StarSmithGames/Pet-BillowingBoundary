using Game.Entities;
using Game.Managers.StorageManager;
using Game.Systems.LocalizationSystem;
using Game.Systems.MarketSystem;
using UnityEngine;
using UnityEngine.Events;

using Zenject;

public class UISkillProperty : MonoBehaviour
{
	public UnityAction<UISkillProperty> onBuyClicked;

	[field: SerializeField] public TMPro.TextMeshProUGUI Title { get; private set; }
	[field: SerializeField] public UIBuyButton BuyButton { get; private set; }

	private ISaveLoad saveLoad;
	private LocalizationSystem localizationSystem;
	private MarketHandler marketHandler;

	[Inject]
	private void Construct(ISaveLoad saveLoad, LocalizationSystem localizationSystem, MarketHandler marketHandler)
	{
		this.saveLoad = saveLoad;
		this.localizationSystem = localizationSystem;
		this.marketHandler = marketHandler;
	}

	private void Start()
	{
		BuyButton.Button.onClick.AddListener(OnBuyClick);
	}

	public void SetProperty(SkillProperty property)
	{
		Title.text = property.GetOutput(localizationSystem);

		if (saveLoad.GetStorage().IsBuyFreeMode.GetData())
		{
			BuyButton.Enable(marketHandler.IsPlayerCanBuy(BFN.Zero));
			BuyButton.SetText(BFN.Zero.ToStringPritty());
		}
		else
		{
			BuyButton.Enable(marketHandler.IsPlayerCanBuy(property.GetCost()));
			BuyButton.SetText(property.GetCost().ToStringPritty());
		}
	}

	private void OnBuyClick()
	{
		onBuyClicked?.Invoke(this);
	}
}