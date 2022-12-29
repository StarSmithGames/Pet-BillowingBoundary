using Game.UI;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
		[field: SerializeField] public RectTransform Rect { get; private set; }

		private List<UIPieceAnimatedBar> pieces = new List<UIPieceAnimatedBar>();

		private UIPieceAnimatedBar.Factory pieceBarFactory;
		private WaveRoad waveRoad;

		[Inject]
		private void Construct(
			UIPieceAnimatedBar.Factory pieceBarFactory,
			WaveRoad waveRoad)
		{
			this.pieceBarFactory = pieceBarFactory;
			this.waveRoad = waveRoad;
		}

		private void Start()
		{
			Content.DestroyChildren();

			waveRoad.onChanged += OnWaveRoadChanged;
			OnWaveRoadChanged();
		}

		private void OnDestroy()
		{
			if(waveRoad!= null)
			{
				waveRoad.onChanged -= OnWaveRoadChanged;
			}
		}

		private void OnWaveRoadChanged()
		{
			LeftWaveLevel.text = $"{waveRoad.CurrentWave.CurrentValue}";
			RightWaveLevel.text = $"{waveRoad.CurrentWave.CurrentValue + 1}";

			CollectionExtensions.Resize((int)waveRoad.CurrentWave.MiddleTargetsBar.CurrentValue, pieces,
			() =>
			{
				var piece = pieceBarFactory.Create();
				piece.transform.SetParent(Content);
				piece.transform.localScale = Vector3.one;

				return piece;
			},
			() =>
			{
				var piece = pieces.Last();
				piece.DespawnIt();

				return piece;
			});

			float width = ((Rect.sizeDelta.x - (5f * waveRoad.CurrentWave.MiddleTargetsBar.MaxValue - 1))/ waveRoad.CurrentWave.MiddleTargetsBar.MaxValue);

			for (int i = 0; i < pieces.Count; i++)
			{
				pieces[i].SetWidth(width);
			}
		}
	}
}