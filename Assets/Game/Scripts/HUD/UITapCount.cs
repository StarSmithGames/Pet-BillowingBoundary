using Game.Entities;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.HUD
{
	public class UITapCount : MonoBehaviour
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Count { get; private set; }

		private Taps taps;

		[Inject]
		private void Construct(Player player)
		{
			this.taps = player.Taps;
		}

		private void Start()
		{
			taps.onChanged += OnTapCountChanged;
			Count.text = taps.Output;
		}

		private void OnDestroy()
		{
			if (taps != null)
			{
				taps.onChanged -= OnTapCountChanged;
			}
		}

		private void OnTapCountChanged()
		{
			Count.text = taps.Output;
		}
	}
}