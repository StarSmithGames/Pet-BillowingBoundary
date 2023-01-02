using Firebase;
using Firebase.Auth;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.AuthenticationSystem
{
	public class FirebaseAuthentication
	{
		public bool IsInitialized { get; private set; } = false;

		public void AuthenticateAnonymously()
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

		public void AuthenticateWithCredential(Credential credential, UnityAction<bool> result)
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