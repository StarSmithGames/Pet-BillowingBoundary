using DG.Tweening;
using UnityEngine;

namespace Game.Systems.MarketSystem
{
    public class UIRemoveADSPurchased : MonoBehaviour
    {
		private void Start()
		{
			transform
				.DORotate(new Vector3(0, 0, 360f), 2.5f, RotateMode.FastBeyond360)
				.SetLoops(-1, LoopType.Restart)
				.SetRelative()
				.SetEase(Ease.Linear);
		}
	}
}