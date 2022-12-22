using UnityEngine;

public abstract class LazySingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<T>();
				if (instance == null)
				{
					instance = new GameObject($"Instance {typeof(T).Name}").AddComponent<T>();
				}
			}

			return instance;
		}
	}
	private static T instance;
}