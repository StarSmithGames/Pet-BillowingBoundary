using Game.Systems.LocalizationSystem;
using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Information
{
	[TitleGroup("Information")]
	[HorizontalGroup("Information/Split", LabelWidth = 100)]
	[VerticalGroup("Information/Split/Left")]
	[PreviewField(ObjectFieldAlignment.Left, Height = 64)]
	[HideLabel]
	public Sprite portrait;

	[VerticalGroup("Information/Split/Right")]
	[HorizontalGroup("Information/Split/Right/SplitName")]
	[LabelText("@NameLabel")]
	public string name;
	[VerticalGroup("Information/Split/Right")]
	[HorizontalGroup("Information/Split/Right/SplitName")]
	[HideLabel]
	public bool isNameId = true;

	[VerticalGroup("Information/Split/Right")]
	[HorizontalGroup("Information/Split/Right/SplitDescription")]
	[LabelText("@DescriptionLabel")]
	public string description;
	[VerticalGroup("Information/Split/Right")]
	[HorizontalGroup("Information/Split/Right/SplitDescription")]
	[HideLabel]
	public bool isDescriptionId = true;

	public bool IsHasPortrait => portrait != null;

	public virtual string GetName()
	{
#if UNITY_EDITOR
		if (isNameId)
		{
			return (!name.IsEmpty() ? LocalizationSystem.TranslateStatic(name, LocalizationSystem.CurrentLocaleStatic) : "");
		}
#endif
		return name;
	}
	public virtual string GetDescription()
	{
#if UNITY_EDITOR
		if (isNameId)
		{
			return (!description.IsEmpty() ? LocalizationSystem.TranslateStatic(description, LocalizationSystem.CurrentLocaleStatic) : "NULL Description");
		}
#endif
		return description;
	}

	private string NameLabel => isNameId ? "Name Id" : "Name";
	private string DescriptionLabel => isDescriptionId ? "Description Id" : "Description";
}