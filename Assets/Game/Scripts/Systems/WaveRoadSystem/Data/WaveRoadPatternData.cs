using Sirenix.OdinInspector;

using System.Collections.Generic;

using UnityEngine;

namespace Game.Systems.WaveRoadSystem
{
    [CreateAssetMenu(fileName = "WaveRoadPatternData", menuName = "Game/Wave Road Pattern")]
    public class WaveRoadPatternData : ScriptableObject
	{
        [ListDrawerSettings(ShowIndexLabels = true, ShowItemCount = true)]
        public List<WaveRoadData> waves = new();

        public RepeatStyle repeat = RepeatStyle.Random;
	}

    public enum RepeatStyle
    {
        Simple,
        Random
    }
}