using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

using System;

using UnityEngine;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.AuthenticationSystem
{
	public class AuthenticationSystem : IInitializable
	{
		private GooglePlayServicesSystem googlePlayServicesSystem;
		private FirebaseAuthentication firebaseAuthentication;

		public AuthenticationSystem(GooglePlayServicesSystem googlePlayServicesSystem, FirebaseAuthentication firebaseAuthentication)
		{
			this.googlePlayServicesSystem = googlePlayServicesSystem;
			this.firebaseAuthentication = firebaseAuthentication;
		}

		public void Initialize()
		{
#if UNITY_EDITOR
			Debug.Log("[AuthenticationSystem] Sign in Editor.");
			return;
#endif

			googlePlayServicesSystem.Authentication(
			(result, auth) =>
			{
				if (auth)
				{
					var authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
					var credential = PlayGamesAuthProvider.GetCredential(authCode);

					firebaseAuthentication.AuthenticationCredential((credential),
					(fireResult) =>
					{
						if (fireResult)
						{
							// user is signed in
							Debug.Log("[AuthenticationSystem] SignInFirebasePlayerGamesAsync.");
						}
						else
						{
							if (FirebaseAuth.DefaultInstance.CurrentUser == null)
							{
								firebaseAuthentication.AuthenticationAnonymously();
								Debug.Log("[AuthenticationSystem] SignInAnonymouslyAsync.");
							}
							else
							{
								// user is signed in
								Debug.Log("[AuthenticationSystem] User is signed in.");
							}
						}
					});
				}
				else
				{
					if (FirebaseAuth.DefaultInstance.CurrentUser == null)
					{
						firebaseAuthentication.AuthenticationAnonymously();
						Debug.Log("[AuthenticationSystem] SignInAnonymouslyAsync.");
					}
					else
					{
						// user is signed in
						Debug.Log("[AuthenticationSystem] User is signed in.");
					}
				}
			});
		}
	}

	public class GooglePlayServicesSystem
	{
		public bool IsAuthenticated { get; private set; } = false;

		public void Authentication(UnityAction<SignInStatus, bool> callback)
		{
			var config = new PlayGamesClientConfiguration.Builder()
			.RequestServerAuthCode(false)//Don't force refresh
			//.EnableSavedGames()
			.Build();

			PlayGamesPlatform.InitializeInstance(config);
			PlayGamesPlatform.Activate();
			PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
			{
				if (result == SignInStatus.Success)
				{
					Debug.Log("[GooglePlayServicesAuthentication] User signed in successfully.");

					PlayGamesPlatform.DebugLogEnabled = true;

					//PlayGamesPlatform.Instance.Events.IncrementEvent("YOUR_EVENT_ID", 1);

					//PlayGamesPlatform.Instance.IncrementAchievement("Cfjewijawiu_QA", 5,
					//(bool success) => {
					//	// handle success or failure
					//});
					//Social.ShowAchievementsUI();

					IsAuthenticated = true;
				}
				else
				{
					Debug.LogError("[GooglePlayServicesAuthentication] User is signed failed.");

					IsAuthenticated = false;
				}

				callback?.Invoke(result, IsAuthenticated);
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
					Debug.LogError($"[FirebaseAuthentication] AuthenticationAnonymously Could not resolve all Firebase dependencies: {dependencyStatus}");
				}
			});

			
		}

		public void AuthenticationCredential(Credential credential, UnityAction<bool> result)
		{
			FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith((task) =>
			{
				var dependencyStatus = task.Result;
				if (dependencyStatus == DependencyStatus.Available)
				{
					FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential).ContinueWith((task) =>
					{
						if (task.IsCanceled)
						{
							Debug.LogError("[FirebaseAuthentication] SignInWithCredentialAsync was canceled.");
							result?.Invoke(false);
							return;
						}
						if (task.IsFaulted)
						{
							Debug.LogError($"[FirebaseAuthentication] SignInWithCredentialAsync encountered an error: {task.Exception}");
							result?.Invoke(false);
							return;
						}

						FirebaseUser newUser = task.Result;
						Debug.Log($"[FirebaseAuthentication] User signed in successfully: {newUser.DisplayName} ({newUser.UserId})");

						result?.Invoke(true);
					});

					IsInitialized = true;
					Debug.Log($"[FirebaseAuthentication] Initialized!");
				}
				else
				{
					Debug.LogError($"[FirebaseAuthentication] SignInWithCredentialAsync Could not resolve all Firebase dependencies: {dependencyStatus}");
					result?.Invoke(false);
				}
			});
		}
	}
}