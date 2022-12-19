using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class UIBar : MonoBehaviour
	{
		public float FillAmount
		{
			get => Bar.fillAmount;
			set
			{
				Bar.fillAmount = value;
				isDirty = true;
			}
		}

		public float FillAmountWhite
		{
			get => WhiteBar.fillAmount;
			set => WhiteBar.fillAmount = value;
		}

		public bool isHasText = false;

		[field: SerializeField] public Image WhiteBar { get; private set; }
		[field: SerializeField] public Image Bar { get; private set; }
		[field: ShowIf("isHasText")]
		[field: SerializeField] public TMPro.TextMeshProUGUI BarText { get; private set; }

		private bool isDirty = false;
		private float speed = 0.005f;

		private void Update()
		{
			if (isDirty)
			{
				if (FillAmountWhite > FillAmount)
				{
					FillAmountWhite -= speed;
				}
				else
				{
					FillAmountWhite = FillAmount;
					isDirty = false;
				}
			}
		}
	}
}