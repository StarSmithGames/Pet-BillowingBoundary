using UnityEngine;
using Zenject;

public interface IPoolable : IPoolable<IMemoryPool>
{
	IMemoryPool Pool { get; }

	void DespawnIt();
}

public abstract class PoolableObject : MonoBehaviour, IPoolable
{
	public IMemoryPool Pool { get => pool; protected set => pool = value; }
	private IMemoryPool pool;

	public virtual void DespawnIt()
	{
		pool?.Despawn(this);
	}

	public virtual void OnSpawned(IMemoryPool pool)
	{
		this.pool = pool;
	}

	public virtual void OnDespawned()
	{
		pool = null;
	}
}