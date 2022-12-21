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

		private Tap tapCount;

		[Inject]
		private void Construct(Player player)
		{
			this.tapCount = player.PlayerSheet.TapCount;
		}

		private void Start()
		{
			tapCount.onChanged += OnTapCountChanged;
			Count.text = tapCount.Output;
		}

		private void OnDestroy()
		{
			if (tapCount != null)
			{
				tapCount.onChanged -= OnTapCountChanged;
			}
		}

		private void OnTapCountChanged()
		{
			Count.text = tapCount.Output;
		}
	}
}