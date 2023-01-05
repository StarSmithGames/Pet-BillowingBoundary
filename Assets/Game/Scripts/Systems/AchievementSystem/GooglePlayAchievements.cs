using Game.Systems.AuthenticationSystem;
using GooglePlayGames;
using System;
using UnityEngine;

namespace Game.Systems.AchievementSystem
{
	public class GooglePlayAchievements
	{
		private GooglePlayAuthentication googlePlaySystem;
		private AnalyticsSystem.AnalyticsSystem analyticsSystem;

		public GooglePlayAchievements(GooglePlayAuthentication googlePlaySystem, AnalyticsSystem.AnalyticsSystem analyticsSystem)
		{
			this.googlePlaySystem = googlePlaySystem;
		}

		/// <param name="progress">0f-100f</param>
		public void Achieve(string id, float progress, Action<bool> callback = null)
		{
			if (!googlePlaySystem.IsAuthenticated) return;

			Social.ReportProgress(id, progress, (result) =>
			{
				analyticsSystem.LogEvent_achievment_get(id);

				callback?.Invoke(result);
			});
		}

		public void UnlockAchievement(string id, Action<bool> callback)
		{
			if (!googlePlaySystem.IsAuthenticated) return;

			PlayGamesPlatform.Instance.UnlockAchievement(id, callback);
		}
	}
}