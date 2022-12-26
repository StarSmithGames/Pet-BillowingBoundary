using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class UIButtonToggle : UIButton
	{
		[field: SerializeField] public Image Image { get; private set; }
		[field: SerializeField] public Image Shadow { get; private set; }
		[SerializeField] private Sprite on;
		[SerializeField] private Sprite off;

		public override void Enable(bool trigger)
		{
			Image.sprite = IsEnable ? on : off;
			if (Shadow != null)
			{
				Shadow.sprite = IsEnable ? on : off;
			}

			base.Enable(trigger);
		}

		[Button(DirtyOnClick = true)]
		private void Check()
		{
			Enable(!IsEnable);
		}
	}
}