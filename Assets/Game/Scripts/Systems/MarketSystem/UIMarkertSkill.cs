using Game.Entities;
using Game.Systems.LocalizationSystem;
using Game.Systems.MarketSystem;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.MarketSystem
{
	public class UIMarkertSkill : MonoBehaviour
	{
		public UnityAction<int> onBuyClick;

		[field: SerializeField] public UISkillProperty First { get; private set; }
		[field: SerializeField] public UISkillProperty Second { get; private set; }
		[field: SerializeField] public UISkillProperty Third { get; private set; }

		public ActiveSkill CurrentSkill { get; private set; }

		private List<UISkillProperty> properties = new List<UISkillProperty>();

		private SignalBus signalBus;
		private Player player;

		[Inject]
		private void Construct(SignalBus signalBus, Player player)
		{
			this.signalBus = signalBus;
			this.player = player;
		}

		private void Start()
		{
			properties.Add(First);
			properties.Add(Second);
			properties.Add(Third);

			for (int i = 0; i < properties.Count; i++)
			{
				properties[i].onBuyClicked += OnBuyClick;
			}

			player.SkillRegistrator.onSelectedSkillChanged += OnSelectedSkillChanged;
			OnSelectedSkillChanged(player.SkillRegistrator.CurrentSkill);
			signalBus?.Subscribe<SignalLocalizationChanged>(OnLocalizationChanged);
			OnLocalizationChanged();
		}

		private void OnDestroy()
		{
			signalBus?.Unsubscribe<SignalLocalizationChanged>(OnLocalizationChanged);

			for (int i = 0; i < properties.Count; i++)
			{
				properties[i].onBuyClicked -= OnBuyClick;
			}

			player.SkillRegistrator.onSelectedSkillChanged -= OnSelectedSkillChanged;
		}

		private void UpdateUI()
		{
			if (CurrentSkill == null || CurrentSkill.IsUnknow) return;

			for (int i = 0; i < properties.Count; i++)
			{
				properties[i].SetProperty(CurrentSkill.GetProperty(i));
			}
		}

		private void OnBuyClick(UISkillProperty property)
		{
			onBuyClick?.Invoke(properties.IndexOf(property));
		}

		private void OnSelectedSkillChanged(ActiveSkill skill)
		{
			CurrentSkill = skill;
			UpdateUI();
		}

		private void OnLocalizationChanged()
		{
			UpdateUI();
		}
	}
}