using Game.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Systems.WaveRoadSystem
{
	public class UIWaveRoad : MonoBehaviour
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI LeftWaveLevel { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI RightWaveLevel { get; private set; }
		[field: SerializeField] public Transform Content { get; private set; }

		private UIPieceAnimatedBar.Factory pieceBarFactory;

		[Inject]
		private void Construct(UIPieceAnimatedBar.Factory pieceBarFactory)
		{
			this.pieceBarFactory = pieceBarFactory;
		}

		private void Start()
		{
			Content.DestroyChildren();
		}
	}
}