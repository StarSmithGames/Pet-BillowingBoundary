using GooglePlayGames;
using GooglePlayGames.BasicApi;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.AuthenticationSystem
{
	public class GooglePlayAuthentication
	{
		public bool IsAuthenticated { get; private set; } = false;

		public void Authenticate(UnityAction<SignInStatus, bool> callback)
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
	}
}