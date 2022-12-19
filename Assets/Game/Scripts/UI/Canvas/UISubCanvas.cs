using UnityEngine;

namespace Game.UI
{
	public abstract class UISubCanvas : UICanvas
	{
		public Transform VFXIndicators
		{
			get
			{
				if (vfxIndicators == null)
				{
					vfxIndicators = transform.Find("VFX/Indicators");
				}

				return vfxIndicators;
			}
		}
		[SerializeField] protected Transform vfxIndicators;
	}
}