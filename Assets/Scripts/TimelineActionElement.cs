using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimelineActionElement : MonoBehaviour
{
	[SerializeField]
	private ActionMetadata _ActionMetadata;

	[SerializeField]
	private TextMeshProUGUI _Index, _Icon, _Label;

	[SerializeField]
	private Graphic _OrderMarker;

	public void Setup(Action action)
	{
		_Index.text = action.Index.ToString();
		_Label.text = _ActionMetadata.ActionOperationsInfo[action.Operation].Name + " " + _ActionMetadata.ActionPartsInfo[action.Part].Name;
		_OrderMarker.color = Color.clear;
		_Icon.text = _ActionMetadata.ActionPartsInfo[action.Part].Icon;
	}
}
