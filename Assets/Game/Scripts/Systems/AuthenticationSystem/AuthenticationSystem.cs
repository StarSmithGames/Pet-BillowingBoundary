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
		private AnalyticsSystem.AnalyticsSystem analyticsSystem;

		public AuthenticationSystem(
			GooglePlayAuthentication googlePlayAuthentication,
			FirebaseAuthentication firebaseAuthentication,
			AchievementSystem.AchievementSystem achievementSystem,
			AnalyticsSystem.AnalyticsSystem analyticsSystem)
		{
			this.googlePlayAuthentication = googlePlayAuthentication;
			this.firebaseAuthentication = firebaseAuthentication;
			this.achievementSystem = achievementSystem;
			this.analyticsSystem = analyticsSystem;
		}

		public void Initialize()
		{
#if UNITY_EDITOR
			Debug.Log("[AuthenticationSystem] Sign in Editor.");
			return;
#endif

			googlePlayAuthentication.Authenticate(
			(auth) =>
			{
				analyticsSystem.LogEvent_authentication($"google_{auth}");

				if (auth)
				{
					var authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
					var credential = PlayGamesAuthProvider.GetCredential(authCode);

					firebaseAuthentication.AuthenticateWithCredential((credential),
					(fireResult) =>
					{
						analyticsSystem.LogEvent_authentication($"firebase_credential_{fireResult}");

						if (fireResult)
						{
							// user is signed in
							Debug.Log("[AuthenticationSystem] SignInFirebasePlayerGamesAsync.");
						}
						else
						{
							if (FirebaseAuth.DefaultInstance.CurrentUser == null)
							{
								firebaseAuthentication.AuthenticateAnonymously((result) =>
								{
									analyticsSystem.LogEvent_authentication($"firebase_anonymously_{result}");
								});
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
						firebaseAuthentication.AuthenticateAnonymously((result) =>
						{
							analyticsSystem.LogEvent_authentication($"firebase_anonymously_{result}");
						});
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