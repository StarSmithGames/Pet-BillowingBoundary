using Game.Entities;
using Game.Systems.MarketSystem;
using UnityEngine;
using UnityEngine.Events;

using Zenject;

public class UISkillProperty : MonoBehaviour
{
	public UnityAction<UISkillProperty> onBuyClicked;

	[field: SerializeField] public TMPro.TextMeshProUGUI Title { get; private set; }
	[field: SerializeField] public UIBuyButton BuyButton { get; private set; }

	private MarketHandler marketHandler;

	[Inject]
	private void Construct(MarketHandler marketHandler)
	{
		this.marketHandler = marketHandler;
	}

	private void Start()
	{
		BuyButton.Button.onClick.AddListener(OnBuyClick);
	}

	public void SetProperty(SkillProperty property)
	{
		Title.text = property.text;
		BuyButton.Enable(marketHandler.IsPlayerCanBuy(property.cost));
		BuyButton.SetText(property.cost.ToStringPritty());
	}

	private void OnBuyClick()
	{
		onBuyClicked?.Invoke(this);
	}
}