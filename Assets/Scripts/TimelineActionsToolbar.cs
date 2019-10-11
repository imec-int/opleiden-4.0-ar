using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineActionsToolbar : MonoBehaviour
{
	[SerializeField]
	private GameObject _ButtonPrefab;

	private ScrollRect _TimelineScrollRect;

	private List<TimelineActionElement> _TimelineActionsElements = new List<TimelineActionElement>();

	private void Awake()
	{
		_TimelineScrollRect = GetComponent<ScrollRect>();
	}

	public void AddTimelineAction(Action action)
	{
		TimelineActionElement timelineAction = GameObject.Instantiate(_ButtonPrefab, _TimelineScrollRect.content).GetComponent<TimelineActionElement>();
		timelineAction.Setup(action);
		_TimelineActionsElements.Add(timelineAction);


		// Make sure the UI is fully up to date to avoid glitching caused by the layout updating the next frame
		LayoutRebuilder.ForceRebuildLayoutImmediate(_TimelineScrollRect.content);
		Canvas.ForceUpdateCanvases();

		// Align the timeline with the last added timeline action
		_TimelineScrollRect.horizontalNormalizedPosition = 1;
	}
}
