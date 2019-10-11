using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimelineActionElement : MonoBehaviour
{
	public static readonly string[] PartNames = { "Error", "Pomp", "Ventiel", "Oliepeil", "Drukmeter" };
	public static readonly string[] PartIcon = { "\uE037", "\uE037", "\uE037", "\uE037", "\uE037" };
	public static readonly string[] HandlingNames = { "Error", "Open", "Sluit", "Check", "Start", "Stop" };

	[SerializeField]
	private TextMeshProUGUI _ID;

	[SerializeField]
	private TextMeshProUGUI _Icon;

	[SerializeField]
	private Graphic _OrderMarker;

	[SerializeField]
	private TextMeshProUGUI _Label;

	public void Setup(Action action)
	{
		_ID.text = action.ID.ToString();
		_Label.text = HandlingNames[(int)action.Handling] + " " + PartNames[(int)action.Part];
		_OrderMarker.color = Color.clear;
		_Icon.text = PartIcon[(int)action.Part];
	}
}
