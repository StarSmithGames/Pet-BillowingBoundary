using Game.Systems.LocalizationSystem;
using Game.Systems.MarketSystem;

using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Entities
{
    public interface ISkill
    {
		SkillData Data { get; }
	}

    public abstract class PassiveSkill : ISkill
    {
		public SkillData Data => data;
		private PassiveSkillData data;
	}
    public abstract class ActiveSkill : MonoBehaviour, ISkill, IPurchasable
	{
		public UnityAction<ActiveSkill> onChanged;
		public abstract SkillData Data { get; }
		public Information Information => Data.information;
		public bool IsUnknow { get; private set; } = false;
		public BuyType BuyType { get; private set; } = BuyType.BUY;

		public Cooldown Cooldown { get; private set; }

		protected bool isHasCooldown = true;
		protected bool isCooldown = false;

		protected Player player;
		protected LocalizationSystem localizationSystem;

		[Inject]
		private void Construct(Player player, LocalizationSystem localizationSystem)
		{
			this.player = player;
			this.localizationSystem = localizationSystem;
		}

		protected virtual void Start()
		{
			player.SkillRegistrator.Registrate(this);

			isHasCooldown = (Data as ActiveSkillData).limitations.isHasCooldown;
			if (isHasCooldown)
			{
				Cooldown = new Cooldown();
			}

			ResetSkill();
		}

		protected virtual void Update()
		{
			if (isHasCooldown)
			{
				if (isCooldown)
				{
					Cooldown.Tick();
					if (Cooldown.Remaining <= 0)
					{
						ResetSkill();
						isCooldown = false;
					}
				}
			}
		}

		public abstract void PurchaseProperty(int index);
		public abstract SkillProperty GetProperty(int index);

		public string GetName(bool isRich = true)
		{
			return localizationSystem.Translate(Information.name);
		}

		public string GetDescription(bool isRich = true)
		{
			return localizationSystem.Translate(Information.description);
		}

		public BFN GetCost()
		{
			return BFN.Zero;
		}

		public void Purchase()
		{
			onChanged?.Invoke(this);
		}

		protected virtual void ResetSkill()
		{
			isCooldown = false;
		}
	}

	public abstract class SkillProperty : AttributeModifiableFloat
	{
		public override float TotalValue => cachedTotalValue;

		public int Level { get; private set; } = 0;

		protected bool isInitialized = false;
		protected BFN currentCost;
		private float cachedTotalValue;

		public SkillProperty(float value) : base(value) { }

		public virtual void LevelUp()
		{
			Level++;

			currentCost = Formule();
			cachedTotalValue = base.TotalValue;
		}

		public virtual string GetOutput(LocalizationSystem localizationSystem)
		{
			return localizationSystem.Translate(LocalizationKey);
		}

		public BFN GetCost()
		{
			if (!isInitialized)
			{
				currentCost = Formule();
				cachedTotalValue = base.TotalValue;
				isInitialized = true;
			}

			return currentCost;
		}

		protected abstract BFN Formule();
	}
}