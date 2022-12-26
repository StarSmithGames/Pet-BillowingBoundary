using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class UIButton : MonoBehaviour
	{
		public bool IsEnable { get; private set; } = true;

		[field: SerializeField] public Button Button { get; private set; }

		protected virtual void Start()
		{
			Button.onClick.AddListener(OnClick);
		}

		private void OnDestroy()
		{
			Button.onClick.RemoveAllListeners();
		}

		public virtual void Enable(bool trigger)
		{
			IsEnable = trigger;
		}

		protected virtual void OnClick()
		{
			Enable(!IsEnable);
		}
	}
}