using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IModifiable<M, T> where M : IModifier
{
	event UnityAction onModifiersChanged;

	T ModifyAddValue { get; }

	List<IModifier> Modifiers { get; }

	bool AddModifier(M modifier);
	bool RemoveModifier(M modifier);

	bool Contains(IModifier modifier);
}

public interface IModifiablePercent
{
	float ModifyPercentValue { get; }
}