using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimelineActionElement : ActionElement
{
	[SerializeField]
	private TextMeshProUGUI _Index;

	[SerializeField]
	private Graphic _OrderMarker;

	public void Setup(Action action)
	{
		Setup(action.Operation,action.Part);
		_Index.text = action.Index.ToString();
		_OrderMarker.color = Color.clear;
	}
}
