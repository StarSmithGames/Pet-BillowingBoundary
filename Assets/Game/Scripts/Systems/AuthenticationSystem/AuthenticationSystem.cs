using Firebase.Auth;
using GooglePlayGames;
using UnityEngine;

using Zenject;

namespace Game.Systems.AuthenticationSystem
{
	public class AuthenticationSystem : IInitializable
	{
		private GooglePlayAuthentication googlePlayAuthentication;
		private FirebaseAuthentication firebaseAuthentication;
		private AchievementSystem.AchievementSystem achievementSystem;

		public AuthenticationSystem(
			GooglePlayAuthentication googlePlaySystem,
			FirebaseAuthentication firebaseAuthentication,
			AchievementSystem.AchievementSystem achievementSystem)
		{
			this.googlePlayAuthentication = googlePlaySystem;
			this.firebaseAuthentication = firebaseAuthentication;
			this.achievementSystem = achievementSystem;
		}

		public void Initialize()
		{
#if UNITY_EDITOR
			Debug.Log("[AuthenticationSystem] Sign in Editor.");
			return;
#endif

			googlePlayAuthentication.Authenticate(
			(result, auth) =>
			{
				if (auth)
				{
					var authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
					var credential = PlayGamesAuthProvider.GetCredential(authCode);

					firebaseAuthentication.AuthenticateWithCredential((credential),
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
								firebaseAuthentication.AuthenticateAnonymously();
								Debug.Log("[AuthenticationSystem] SignInAnonymouslyAsync.");
							}
							else
							{
								// user is signed in
								Debug.Log("[AuthenticationSystem] User is signed in.");
							}
						}
					});

					achievementSystem.Achive_achievement_newkid_authenticated();
				}
				else
				{
					if (FirebaseAuth.DefaultInstance.CurrentUser == null)
					{
						firebaseAuthentication.AuthenticateAnonymously();
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
}