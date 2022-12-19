using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IAttribute : IValue<float>
{
	string Output { get; }

	string LocalizationKey { get; }
}

public abstract partial class Attribute : IAttribute
{
	public event UnityAction onChanged;

	public virtual string Output => CurrentValue.ToString();

	public virtual string LocalizationKey => "sheet.";

	public virtual float CurrentValue
	{
		get => currentValue;
		set
		{
			currentValue = value;
			onChanged?.Invoke();
		}
	}
	protected float currentValue;

	public Attribute(float currentValue)
	{
		this.currentValue = currentValue;
	}
}

public abstract partial class AttributeBar : Attribute, IBar
{
	public override string Output => $"{Math.Round(CurrentValue)} / {Math.Round(MaxValue)}";

	public override float CurrentValue
	{
		get => currentValue;
		set
		{
			base.CurrentValue = Mathf.Clamp(value, MinValue, MaxValue);
		}
	}

	public virtual float MaxValue
	{
		get => maxValue;
		set
		{
			maxValue = value;
			base.CurrentValue = Mathf.Clamp(currentValue, MinValue, MaxValue);
		}
	}
	protected float maxValue;

	public virtual float MinValue { get; protected set; }

	public float PercentValue => CurrentValue / MaxValue;

	protected AttributeBar(float value, float min, float max) : base(value)
	{
		this.maxValue = max;
		this.MinValue = min;
		this.CurrentValue = value;
	}
}