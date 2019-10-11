﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionElement : MonoBehaviour
{
	[SerializeField]
	private ActionMetadata _ActionMetadata;

	[SerializeField]
	private TextMeshProUGUI _Icon, _Label;

	public void Setup(Operation operation, Part part)
	{
		Setup(_ActionMetadata.ActionOperationsInfo[operation].Name + " " + _ActionMetadata.ActionPartsInfo[part].Name,
		 _ActionMetadata.ActionPartsInfo[part].Icon);
	}

	public void Setup(string label, string icon)
	{
		_Label.text = label;
		_Icon.text = icon;
	}
}
