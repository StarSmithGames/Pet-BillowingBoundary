using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IModifiable<M> where M : Modifier<float>
{
	event UnityAction onModifiersChanged;

	float TotalValue { get; }
	float ModifyAddValue { get; }
	float ModifyPercentValue { get; }

	List<M> Modifiers { get; }

	bool AddModifier(M modifier);
	bool RemoveModifier(M modifier);

	bool Contains(M modifier);
}