using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class UIButtonToggle : MonoBehaviour
	{
		public bool IsEnable { get; private set; } = true;

		[field: SerializeField] public Button Button { get; private set; }
		[field: SerializeField] public Image Image { get; private set; }
		[SerializeField] private Sprite on;
		[SerializeField] private Sprite off;

		private void Start()
		{
			Button.onClick.AddListener(OnClick);
		}

		private void OnDestroy()
		{
			Button.onClick.RemoveAllListeners();
		}

		public void Enable(bool trigger)
		{
			IsEnable = trigger;
			Image.sprite = IsEnable ? on : off;
		}

		private void OnClick()
		{
			Enable(!IsEnable);
		}

		[Button(DirtyOnClick = true)]
		private void Check()
		{
			Enable(!IsEnable);
		}
	}
}