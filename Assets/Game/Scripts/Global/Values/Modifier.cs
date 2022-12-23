using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IModifier { }

public abstract class Modifier<T> : IModifier, IReadOnlyValue<T>
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

public abstract class AttributeModifierBFN : Modifier<BFN>
{
	protected AttributeModifierBFN(BFN value) : base(value) { }
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

#region BFNModifiiers
public class AddModifierBFN : AttributeModifierBFN
{
	public AddModifierBFN(BFN value) : base(value) { }
}
#endregion