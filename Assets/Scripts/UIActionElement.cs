using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIActionElement : MonoBehaviour
{
	[SerializeField]
	protected ActionMetadata _ActionMetadata;

	[SerializeField]
	protected TextMeshProUGUI _Icon, _Label;

	[SerializeField]
	protected Button _AssociatedButton;
	
	public Button AssociatedButton
	{
		get {return _AssociatedButton;}
	}

	public void Setup(Operation operation, Part part)
	{
		Setup(_ActionMetadata.ActionOperationsInfo[operation].Name + " " + _ActionMetadata.ActionPartsInfo[part].Name,
		 _ActionMetadata.ActionPartsInfo[part].Icon);
	}

	public void Setup(string label, string icon)
	{
		_Label.text = label;
		_Icon.text = icon.ToUnicodeForTMPro();
	}
}
