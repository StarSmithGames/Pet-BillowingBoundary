using Game.Entities;
using System;
using UnityEngine.Events;
using Zenject;

namespace Game.Systems.MarketSystem
{
	public class MarketHandler : IInitializable, IDisposable
	{
		public UnityAction onValuableChanged;

		public bool IsCanBoughtSomething { get; private set; } = false;

		private Gold gold;
		private BonusRegistrator br;

		private Player player;

		public MarketHandler(Player player)
		{
			this.player = player;
		}

		public void Initialize()
		{
			br = player.BonusRegistrator;
			gold = player.Gold;
			gold.onChanged += OnGoldChanged;
		}

		public void Dispose()
		{
			if(gold != null)
			{
				gold.onChanged += OnGoldChanged;
			}
		}

		public void Reset()
		{
			IsCanBoughtSomething = false;
		}

		public bool IsPlayerCanBuy(IPurchasable purchasable)
		{
			return IsPlayerCanBuy(purchasable.GetCost());
		}

		public bool IsPlayerCanBuy(BFN bfn)
		{
			return gold.CurrentValue >= bfn;
		}

		private void OnGoldChanged()
		{
			for (int i = 0; i < br.registers.Count; i++)
			{
				if (IsPlayerCanBuy(br.registers[i]))
				{
					IsCanBoughtSomething = true;
					onValuableChanged?.Invoke();
					return;
				}
			}

			IsCanBoughtSomething = false;
			onValuableChanged?.Invoke();
		}
	}
}