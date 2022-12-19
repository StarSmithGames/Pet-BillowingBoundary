using UnityEngine;

public static class CanvasGroupExtensions
{
	public static void Enable(this CanvasGroup canvasGroup, bool trigger, bool withAlpha = true)
	{
		if (withAlpha)
		{
			canvasGroup.alpha = trigger ? 1f : 0f;
		}
		canvasGroup.blocksRaycasts = trigger;
		canvasGroup.interactable = trigger;
	}
}