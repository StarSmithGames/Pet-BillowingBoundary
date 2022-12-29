using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

using System;

using UnityEngine;

using Zenject;

namespace Game.Systems.AuthenticationSystem
{
	public class AuthenticationSystem : IInitializable
	{
		private FirebaseAuthentication authentication;

		public AuthenticationSystem(FirebaseAuthentication authentication)
		{
			this.authentication = authentication;
		}

		public void Initialize()
		{
#if UNITY_EDITOR
			Debug.Log("[AuthenticationSystem] Sign in Editor.");
			return;
#endif
			if (FirebaseAuth.DefaultInstance.CurrentUser == null)
			{
				Debug.Log("[AuthenticationSystem] SignInAnonymouslyAsync.");
				authentication.AuthenticationAnonymously();
			}
			else
			{
				// user is signed in, continue to your game
				Debug.Log("[AuthenticationSystem] User is signed in.");
			}
		}
	}

	public class GooglePlayServicesSystem : IInitializable, IDisposable
	{
		public bool IsAuthenticated { get; private set; } = false;

		private SignalBus signalBus;

		public GooglePlayServicesSystem(SignalBus signalBus)
		{
			this.signalBus = signalBus;
		}

		public void Initialize()
		{
			Authentication();
		}

		public void Dispose()
		{
		}

		public void Authentication()
		{
			var config = new PlayGamesClientConfiguration.Builder()
			//.EnableSavedGames()
			.Build();

			PlayGamesPlatform.InitializeInstance(config);
			PlayGamesPlatform.Activate();
			PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
			{
				if (result == SignInStatus.Success)
				{
					Debug.Log("[GooglePlayServicesAuthentication] User signed in successfully.");

					//PlayGamesPlatform.Instance.Events.IncrementEvent("YOUR_EVENT_ID", 1);

					PlayGamesPlatform.DebugLogEnabled = true;

					PlayGamesPlatform.Instance.IncrementAchievement("Cfjewijawiu_QA", 5,
					(bool success) => {
						// handle success or failure
					});
					//Social.ShowAchievementsUI();

					IsAuthenticated = true;
				}
				else
				{
					Debug.LogError("[GooglePlayServicesAuthentication] User is signed failed.");

					IsAuthenticated = false;
				}
			});
		}

		/// <param name="progress">0f-100f</param>
		public void Achieve(string id, float progress, Action<bool> callback)
		{
			if (!IsAuthenticated) return;

			Social.ReportProgress(id, progress, callback);
		}

		public void UnlockAchievement(string id, Action<bool> callback)
		{
			if (!IsAuthenticated) return;

			PlayGamesPlatform.Instance.UnlockAchievement(id, callback);
		}
	}


	public class FirebaseAuthentication
	{
		public bool IsInitialized { get; private set; } = false;

		public void AuthenticationAnonymously()
		{
			FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith((task) =>
			{
				var dependencyStatus = task.Result;
				if (dependencyStatus == DependencyStatus.Available)
				{
					var app = FirebaseApp.DefaultInstance;

					FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync().ContinueWith(task => {
						if (task.IsCanceled)
						{
							Debug.LogError("[FirebaseAuthentication] SignInAnonymouslyAsync was canceled.");
							return;
						}
						if (task.IsFaulted)
						{
							Debug.LogError("[FirebaseAuthentication] SignInAnonymouslyAsync encountered an error: " + task.Exception);
							return;
						}

						FirebaseUser newUser = task.Result;

						Debug.Log($"[FirebaseAuthentication] User signed in successfully: {newUser.DisplayName} {newUser.UserId}");
					});

					IsInitialized = true;
					Debug.Log($"[FirebaseAuthentication] Initialized!");
				}
				else
				{
					Debug.LogError($"[FirebaseAuthentication] Could not resolve all Firebase dependencies: {dependencyStatus}");
				}
			});

			
		}
	}
}