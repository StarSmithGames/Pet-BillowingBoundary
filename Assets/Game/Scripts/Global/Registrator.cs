using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

public class Registrator<T>
{
	public event UnityAction onCollectionChanged;
	public event UnityAction<T> onItemAdded;
	public event UnityAction<T> onItemRemoved;

	public List<T> registers;

	public Registrator()
	{
		registers = new List<T>();
	}

	public void SetCollection(IEnumerable<T> items)
	{
		registers = new List<T>(items);

		onCollectionChanged?.Invoke();
	}

	public virtual bool Registrate(T register)
	{
		if (!registers.Contains(register))
		{
			registers.Add(register);

			onItemAdded?.Invoke(register);
			onCollectionChanged?.Invoke();

			return true;
		}

		return false;
	}

	public virtual void Registrate(IEnumerable<T> items)
	{
		foreach (var register in items)
		{
			if (!registers.Contains(register))
			{
				registers.Add(register);
			}
		}

		onCollectionChanged?.Invoke();
	}

	public virtual bool UnRegistrate(T register)
	{
		if (registers.Contains(register))
		{
			registers.Remove(register);

			onItemRemoved?.Invoke(register);
			onCollectionChanged?.Invoke();

			return true;
		}

		return false;
	}


	public REGISTR GetAs<REGISTR>() where REGISTR : class, T
	{
		return Get<REGISTR>() as REGISTR;
	}

	public T Get<REGISTR>() where REGISTR : T
	{
		if (ContainsType<REGISTR>())
		{
			return registers.Where((x) => x is REGISTR).FirstOrDefault();
		}

		throw new System.Exception($"REGISTRATOR DOESN'T CONTAINS {typeof(REGISTR)} ERROR");
	}

	public bool ContainsType<REGISTR>() where REGISTR : T
	{
		return registers.OfType<REGISTR>().Any();
	}

	public bool ContainsType<REGISTR>(out REGISTR registr) where REGISTR : T
	{
		registr = registers.OfType<REGISTR>().FirstOrDefault();
		return registr != null;
	}
}