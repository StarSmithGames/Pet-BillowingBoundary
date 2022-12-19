using UnityEngine;

namespace Game.UI
{
	public abstract class UISubCanvas : UICanvas
	{
		public Transform VFX
		{
			get
			{
				if (vfx == null)
				{
					vfx = transform.Find("VFX");
				}

				return vfx;
			}
		}
		[SerializeField] protected Transform vfx;
	}
}