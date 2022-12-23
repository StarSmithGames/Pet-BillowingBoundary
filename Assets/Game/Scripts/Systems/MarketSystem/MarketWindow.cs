using DG.Tweening;

using Game.Entities;
using Game.UI;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Zenject;

using static UnityEditor.Progress;

namespace Game.Systems.MarketSystem
{
	public class MarketWindow : WindowBase
	{
		[field: SerializeField] public Button Blank { get; private set; }
		[field: SerializeField] public Button Close { get; private set; }
		[field: SerializeField] public RectTransform WindowUp { get; private set; }
		[field: SerializeField] public RectTransform WindowDown { get; private set; }
		[field: Space]
		[field: SerializeField] public Transform ContentTop0 { get; private set; }
		[field: SerializeField] public Transform ContentTop1 { get; private set; }

		private List<UIMarketItem> marketItems0 = new List<UIMarketItem>();

		private bool isOpenned = false;

		private UISubCanvas subCanvas;
		private Player player;
		private UIMarketItem.Factory marketItemFactory;

		[Inject]
		private void Construct(UISubCanvas subCanvas, Player player,
			UIMarketItem.Factory marketItemFactory)
		{
			this.subCanvas = subCanvas;
			this.player = player;
			this.marketItemFactory = marketItemFactory;
		}
		
		private void Start()
		{
			Enable(false);

			ContentTop0.DestroyChildren();
			ContentTop1.DestroyChildren();

			Close.onClick.AddListener(OnClosed);
			Blank.onClick.AddListener(OnClosed);

			subCanvas.WindowsRegistrator.Registrate(this);

			player.BonusRegistrator.onCollectionChanged += OnBonusCollectionChanged;
			if(player.BonusRegistrator.registers.Count != 0)
			{
				OnBonusCollectionChanged();
			}
		}

		private void OnDestroy()
		{
			Close?.onClick.RemoveAllListeners();
			Blank?.onClick.RemoveAllListeners();

			subCanvas?.WindowsRegistrator.UnRegistrate(this);

			if(player != null)
			{
				player.BonusRegistrator.onCollectionChanged -= OnBonusCollectionChanged;
			}
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

		private void OnBonusCollectionChanged()
		{
			CollectionExtensions.Resize(player.BonusRegistrator.registers, marketItems0,
			() =>
			{
				var item = marketItemFactory.Create();
				item.transform.SetParent(ContentTop0);
				item.transform.localScale = Vector3.one;

				item.onBuyClick += OnBuyClicked;

				return item;
			},
			() =>
			{
				var last = marketItems0.Last();
				last.SetBonus(null);
				last.onBuyClick -= OnBuyClicked;
				last.DespawnIt();

				return last;
			});

			for (int i = 0; i < marketItems0.Count; i++)
			{
				marketItems0[i].SetBonus(player.BonusRegistrator.registers[i]);
				marketItems0[i].Separator.SetActive(i < marketItems0.Count - 1);
			}

			Debug.LogError("OnBonusCollectionChanged");
		}

		private void OnBuyClicked(UIMarketItem marketItem)
		{
			marketItem.CurrentBonus.LevelUp();
		}

		private void OnClosed()
		{
			Hide();
		}
	}
}