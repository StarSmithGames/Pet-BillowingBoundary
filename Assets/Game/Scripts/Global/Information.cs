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


	public string GetName(LocalizationSystem localizationSystem)
	{
		return isNameId ? localizationSystem.Translate(name) : name;
	}

	public string GetDescription(LocalizationSystem localizationSystem)
	{
		return isDescriptionId ? localizationSystem.Translate(description) : description;
	}

#if UNITY_EDITOR
	public virtual string GetName()
	{
		if (isNameId)
		{
			return (!name.IsEmpty() ? LocalizationSystem.TranslateStatic(name, LocalizationSystem.CurrentLocaleStatic) : "NULL Name");
		}
		return name;
	}
	public virtual string GetDescription()
	{
		if (isNameId)
		{
			return (!description.IsEmpty() ? LocalizationSystem.TranslateStatic(description, LocalizationSystem.CurrentLocaleStatic) : "NULL Description");
		}
		return description;
	}
#endif

	private string NameLabel => isNameId ? "Name Id" : "Name";
	private string DescriptionLabel => isDescriptionId ? "Description Id" : "Description";
}