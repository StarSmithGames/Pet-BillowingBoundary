using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Modifier<T> : IReadOnlyValue<T>
{
	public event UnityAction onChanged;

	public T CurrentValue { get; private set; }

	public Modifier(T value)
	{
		CurrentValue = value;
	}

	public void SetValue(T value)
	{
		CurrentValue = value;
		onChanged?.Invoke();
	}
}

public abstract class AttributeModifier : Modifier<float>
{
	protected AttributeModifier(float value) : base(value) { }
}

#region Modifiiers
public class AddModifier : AttributeModifier
{
	public AddModifier(float value) : base(value) { }
}

public class PercentModifier : AttributeModifier
{
	public PercentModifier(float value) : base(value) { }
}
#endregion