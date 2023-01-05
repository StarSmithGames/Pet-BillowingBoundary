using Firebase;
using Firebase.Analytics;
using Firebase.Crashlytics;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Systems.AnalyticsSystem
{
	public interface IAnalyticsGroup
	{
		void LogEvent(string id);
		void LogEvent(string id, Dictionary<string, object> parameters);
	}

	public class AmplitudeAnalyticsGroup : IAnalyticsGroup
	{
		private Amplitude instance;

		public AmplitudeAnalyticsGroup()
		{
			instance = Amplitude.Instance;
			instance.logging = true;
			instance.trackSessionEvents(true);
			instance.init("9b8603d0a7aac332ca6873cc1490f51a");
		}

		public void LogEvent(string id)
		{
			instance.logEvent(id);
		}

		public void LogEvent(string id, Dictionary<string, object> parameters)
		{
			instance.logEvent(id, parameters);
		}
	}

	public class FirebaseAnalyticsGroup : IAnalyticsGroup
	{
		public bool IsInitialized { get; private set; } = false;

		public FirebaseAnalyticsGroup()
		{
			FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith((task) =>
			{
				var dependencyStatus = task.Result;
				if (dependencyStatus == DependencyStatus.Available)
				{
					var app = FirebaseApp.DefaultInstance;

					FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
					Crashlytics.IsCrashlyticsCollectionEnabled = true;

					IsInitialized = true;
					Debug.Log($"[FirebaseAnalyticsGroup] Initialized!");
				}
				else
				{
					Debug.LogError($"[FirebaseAnalyticsGroup] Could not resolve all Firebase dependencies: {dependencyStatus}");
				}
			});
		}

		public void LogEvent(string id)
		{
			if (!IsInitialized) return;

			FirebaseAnalytics.LogEvent(id);
		}

		public void LogEvent(string id, Dictionary<string, object> parameters)
		{
			if (!IsInitialized) return;

			FirebaseAnalytics.LogEvent(id, IntoParameterArray(parameters));
		}

		private Parameter[] IntoParameterArray(Dictionary<string, object> parameters)
		{
			return parameters?.Where(x => x.Value != null).Select(x => new Parameter(x.Key, x.Value.ToString())).ToArray() ?? new Parameter[0];
		}
		private Dictionary<string, string> IntoStringDictionary(Dictionary<string, object> parameters)
		{
			return parameters.ToDictionary(x => x.Key, x => x.Value.ToString());
		}
	}
}