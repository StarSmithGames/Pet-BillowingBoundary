using DG.Tweening;

using Game.Entities;
using Game.Managers.AudioManager;
using Game.Managers.VibrationManager;
using Game.Systems.PremiumMarketSystem;
using Game.UI;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public class MarketWindow : WindowBase
	{
		[field: SerializeField] public Button Blank { get; private set; }
		[field: SerializeField] public Button Close { get; private set; }
		[field: Header("Components")]
		[field: SerializeField] public RectTransform WindowUp { get; private set; }
		[field: SerializeField] public RectTransform WindowDown { get; private set; }
		[field: Space]
		[field: SerializeField] public ScrollRect ScrollRectTop0 { get; private set; }
		[field: SerializeField] public Transform ContentTop0 { get; private set; }
		[field: SerializeField] public ScrollRect ScrollRectTop1 { get; private set; }
		[field: SerializeField] public Transform ContentTop1 { get; private set; }
		[field: SerializeField] public TabsSystem TabsSystemTop { get; private set; }
		[field: Space]
		[field: SerializeField] public TabsSystem TabsSystemBottom { get; private set; }
		[field: SerializeField] public UIMarkertSkill MarkertSkill { get; private set; }

		private List<UIMarketBonusItem> marketItems0 = new List<UIMarketBonusItem>();
		private List<UIMarketSkillItem> marketItems1 = new List<UIMarketSkillItem>();

		private bool isOpenned = false;

		private UISubCanvas subCanvas;
		private Player player;
		private UIMarketBonusItem.Factory marketItemBonusFactory;
		private UIMarketSkillItem.Factory marketItemSkillFactory;
		private AudioManager audioManager;
		private VibrationManager vibrationManager;

		[Inject]
		private void Construct(UISubCanvas subCanvas, Player player,
			UIMarketBonusItem.Factory marketItemBonusFactory,
			UIMarketSkillItem.Factory marketItemSkillFactory,
			AudioManager audioManager,
			VibrationManager vibrationManager)
		{
			this.subCanvas = subCanvas;
			this.player = player;
			this.marketItemBonusFactory = marketItemBonusFactory;
			this.marketItemSkillFactory = marketItemSkillFactory;
			this.audioManager = audioManager;
			this.vibrationManager = vibrationManager;
		}
		
		private void Start()
		{
			Enable(false);

			ContentTop0.DestroyChildren();
			ContentTop1.DestroyChildren();

			Close.onClick.AddListener(OnClosed);
			Blank.onClick.AddListener(OnClosed);

			subCanvas.WindowsRegistrator.Registrate(this);

			player.BonusRegistrator.onCollectionChanged += OnMarketCollectionChanged0;
			player.SkillRegistrator.onCollectionChanged += OnMarketCollectionChanged1;
			if (marketItems0.Count == 0)
			{
				OnMarketCollectionChanged0();
			}

			if (marketItems1.Count == 0)
			{
				OnMarketCollectionChanged1();
			}
			
			MarkertSkill.onBuyClick += OnBuyClicked;
		}

		private void OnDestroy()
		{
			Close?.onClick.RemoveAllListeners();
			Blank?.onClick.RemoveAllListeners();

			subCanvas?.WindowsRegistrator.UnRegistrate(this);

			if(player != null)
			{
				player.BonusRegistrator.onCollectionChanged -= OnMarketCollectionChanged0;
				player.SkillRegistrator.onCollectionChanged -= OnMarketCollectionChanged1;
			}

			MarkertSkill.onBuyClick += OnBuyClicked;
		}

		public override void Show(UnityAction callback = null)
		{
			IsInProcess = true;
			isOpenned = true;

			CanvasGroup.alpha = 0f;
			CanvasGroup.Enable(true, false);
			WindowUp.anchoredPosition = new Vector2(WindowUp.anchoredPosition.x, WindowUp.sizeDelta.y / 2);//up
			WindowDown.anchoredPosition = new Vector2(WindowDown.anchoredPosition.x, -(WindowDown.sizeDelta.y / 2));//down
			IsShowing = true;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(1f, 0.2f))
				.Join(WindowUp.DOAnchorPosY(-(WindowUp.sizeDelta.y / 2), 0.25f, true).SetEase(Ease.OutBounce))
				.Join(WindowDown.DOAnchorPosY(WindowDown.sizeDelta.y / 2, 0.25f))
				.AppendCallback(() =>
				{
					callback?.Invoke();
					IsInProcess = false;
				});
		}

		public override void Hide(UnityAction callback = null)
		{
			IsInProcess = true;
			isOpenned = false;

			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(CanvasGroup.DOFade(0f, 0.15f))
				.Join(WindowUp.DOAnchorPosY(WindowUp.sizeDelta.y / 2, 0.15f))
				.Join(WindowDown.DOAnchorPosY(-(WindowDown.sizeDelta.y / 2), 0.15f))
				.AppendCallback(() =>
				{
					CanvasGroup.Enable(false);
					IsShowing = false;
					callback?.Invoke();

					IsInProcess = false;
				});
		}

		private void OnMarketCollectionChanged0()
		{
			//Resize
			CollectionExtensions.Resize(player.BonusRegistrator.registers, marketItems0,
			() =>
			{
				var item = marketItemBonusFactory.Create();
				item.transform.SetParent(ContentTop0);
				item.transform.localScale = Vector3.one;

				item.onBuyClick += OnBuyClicked;

				return item;
			},
			() =>
			{
				var last = marketItems0.Last();
				last.SetPurchasing(null);
				last.onBuyClick -= OnBuyClicked;
				last.DespawnIt();

				return last;
			});

			//Update UI
			for (int i = 0; i < marketItems0.Count; i++)
			{
				marketItems0[i].SetPurchasing(player.BonusRegistrator.registers[i]);
				marketItems0[i].Separator.SetActive(i < marketItems0.Count - 1);
			}
		}

		private void OnMarketCollectionChanged1()
		{
			CollectionExtensions.Resize(player.SkillRegistrator.registers, marketItems1,
			() =>
			{
				var item = marketItemSkillFactory.Create();
				item.transform.SetParent(ContentTop1);
				item.transform.localScale = Vector3.one;

				item.onBuyClick += OnBuyClicked;

				return item;
			},
			() =>
			{
				var last = marketItems1.Last();
				last.SetPurchasing(null);
				last.onBuyClick -= OnBuyClicked;
				last.DespawnIt();

				return last;
			});

			for (int i = 0; i < marketItems1.Count; i++)
			{
				marketItems1[i].SetPurchasing(player.SkillRegistrator.registers[i]);
				marketItems1[i].Separator.SetActive(i < marketItems1.Count - 1);
			}
		}

		private void OnBuyClicked(int skillPropertyIndex)
		{
			var property = MarkertSkill.CurrentSkill.GetProperty(skillPropertyIndex);

			if (player.Gold.CurrentValue < property.GetCost())
			{
				subCanvas.WindowsRegistrator.Show<PremiumMarketWindow>();
				return;
			}

			player.Gold.CurrentValue -= property.GetCost();
			MarkertSkill.CurrentSkill.PurchaseProperty(skillPropertyIndex);
		}

		private void OnBuyClicked(UIMarketItem marketItem)
		{
			if (player.Gold.CurrentValue < marketItem.CurrentPurchase.GetCost())
			{
				subCanvas.WindowsRegistrator.Show<PremiumMarketWindow>();
				return;
			}

			player.Gold.CurrentValue -= marketItem.CurrentPurchase.GetCost();
			marketItem.CurrentPurchase.Purchase();
		}

		private void OnClosed()
		{
			Hide(() =>
			{
				ScrollRectTop0.ScrollToTop();
				ScrollRectTop1.ScrollToTop();
				TabsSystemTop.SelectFirst();
				TabsSystemBottom.SelectFirst();
			});

			audioManager.PlayButtonClick();
			vibrationManager.Vibrate();
		}
	}
}