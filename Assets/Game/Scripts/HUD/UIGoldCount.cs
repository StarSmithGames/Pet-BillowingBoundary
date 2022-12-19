using Game.Entities;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.HUD
{
	public class UIGoldCount : MonoBehaviour
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Count { get; private set; }

		private Gold gold;

		[Inject]
		private void Construct(Player player)
		{
			this.gold = player.Gold;
		}

		private void Start()
		{
			gold.onChanged += OnTapCountChanged;
			Count.text = gold.Output;
		}

		private void OnDestroy()
		{
			if (gold != null)
			{
				gold.onChanged -= OnTapCountChanged;
			}
		}

		private void OnTapCountChanged()
		{
			Count.text = gold.Output;
		}
	}
}