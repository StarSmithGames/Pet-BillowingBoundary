using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
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