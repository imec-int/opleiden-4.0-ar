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

	private static string GetUnicodeForTMPro(string iconUnicode)
	{
		if (iconUnicode.Contains(@"\u"))
			return iconUnicode;

		string result = "\uE037";
		try
		{
			int unicode = int.Parse(iconUnicode, System.Globalization.NumberStyles.HexNumber);
			result = char.ConvertFromUtf32(unicode);
		}
		catch(FormatException e)
		{
			result = iconUnicode;
			Debug.LogWarning($"Failed to parse {iconUnicode} for TMPro");
		}	
		return result;
	}
	
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
		_Icon.text = GetUnicodeForTMPro(icon);
	}
}
