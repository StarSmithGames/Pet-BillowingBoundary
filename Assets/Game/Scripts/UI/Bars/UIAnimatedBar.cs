using Sirenix.OdinInspector;
using UnityEngine;

public class UIAnimatedBar : MonoBehaviour
{
	public float FillAmount
	{
		get => Bar.anchorMax.x;
		set
		{
			Bar.anchorMax = new Vector2(value, Bar.anchorMax.y);
			//isDirty = true;
		}
	}

	[field: SerializeField] public RectTransform Bar { get; private set; }
	[SerializeField] private bool isHasText = false;
	[field: ShowIf("isHasText")]
	[field: SerializeField] public TMPro.TextMeshProUGUI BarText { get; private set; }
}