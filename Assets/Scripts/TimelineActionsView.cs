using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineActionsView : MonoBehaviour
{
	[SerializeField]
	private TimelineActionWidget _ButtonPrefab;

	private ScrollRect _TimelineScrollRect;

	private List<TimelineActionWidget> _TimelineActionsElements = new List<TimelineActionWidget>();

	private void Awake()
	{
		_TimelineScrollRect = GetComponent<ScrollRect>();
	}

	public void AddTimelineAction(ActionData action)
	{
		TimelineActionWidget timelineAction = GameObject.Instantiate(_ButtonPrefab, _TimelineScrollRect.content).GetComponent<TimelineActionWidget>();
		timelineAction.Setup(action);
		_TimelineActionsElements.Add(timelineAction);


		// Make sure the UI is fully up to date to avoid glitching caused by the layout updating the next frame
		LayoutRebuilder.ForceRebuildLayoutImmediate(_TimelineScrollRect.content);
		Canvas.ForceUpdateCanvases();

		// Align the timeline with the last added timeline action
		_TimelineScrollRect.horizontalNormalizedPosition = 1;

		action.Delete += ActionDeleted;
	}

	private void ActionDeleted(ActionData action)
	{
		_TimelineActionsElements.RemoveAt((int)action.Index - 1);
	}
}
