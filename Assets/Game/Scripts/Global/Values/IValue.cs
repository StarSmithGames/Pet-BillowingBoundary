using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IValue<T> : IReadOnlyValue<T>
{
	new T CurrentValue { get; set; }
}
public interface IReadOnlyValue<T> : IObservableValue
{
	T CurrentValue { get; }
}
public interface IObservableValue
{
	event UnityAction onChanged;
}

public interface IBounded<T>
{
	T MinValue { get; }
	T MaxValue { get; }
}

public interface IBar<T> : IBounded<T> where T : struct
{
	float PercentValue { get; }
}