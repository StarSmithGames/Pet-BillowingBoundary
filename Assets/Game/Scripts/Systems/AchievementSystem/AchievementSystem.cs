using Game.Systems.AuthenticationSystem;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.AchievementSystem
{
	public class AchievementSystem
	{
		private GooglePlayAchievements googlePlayAchievements;

		public AchievementSystem(GooglePlayAchievements googlePlayAchievements)
		{
			this.googlePlayAchievements = googlePlayAchievements;
		}


		public void Achive_achievement_newkid_authenticated()
		{
			googlePlayAchievements.Achieve(GPSAchievements.achievement_newkid_authenticated, 100);
		}

		public void Achive_achievement_100_times()
		{
			googlePlayAchievements.Achieve(GPSAchievements.achievement_100_times, 100);
		}

		public void Achive_achievement_1000_times()
		{
			googlePlayAchievements.Achieve(GPSAchievements.achievement_1000_times, 100);
		}

		public void Achive_achievement_10000_times()
		{
			googlePlayAchievements.Achieve(GPSAchievements.achievement_10000_times, 100);
		}

		public void Achive_achievement_rapid_fire()
		{
			googlePlayAchievements.Achieve(GPSAchievements.achievement_rapid_fire, 100);
		}

		public void Achive_achievement_overkill()
		{
			googlePlayAchievements.Achieve(GPSAchievements.achievement_overkill, 100);
		}

		public void Achive_achievement_monster()
		{
			googlePlayAchievements.Achieve(GPSAchievements.achievement_monster, 100);
		}

		public void Achive_achievement_oh_wow_okay()
		{
			googlePlayAchievements.Achieve(GPSAchievements.achievement_oh_wow_okay, 100);
		}

		public void Achive_achievement_iam_a_rich_now()
		{
			googlePlayAchievements.Achieve(GPSAchievements.achievement_iam_a_rich_now, 100);
		}

		public void Achive_achievement_the_richest_man_on_the_planet()
		{
			googlePlayAchievements.Achieve(GPSAchievements.achievement_the_richest_man_on_the_planet, 100);
		}
	}
}