using Firebase;

using UnityEngine;

using Zenject;

namespace Game.Systems.AnalyticsSystem
{
	public class AnalyticsSystem : IInitializable
	{
		public bool IsInitialized { get; private set; } = false;

		public void Initialize()
		{
			FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
				var dependencyStatus = task.Result;
				if (dependencyStatus == DependencyStatus.Available)
				{
					var app = FirebaseApp.DefaultInstance;

					IsInitialized = true;
				}
				else
				{
					Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
				}
			});
		}
	}
}