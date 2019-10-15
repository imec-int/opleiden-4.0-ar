using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class TimelineActionsView : MonoBehaviour
{
	[SerializeField]
	private TimelineActionWidget _ButtonPrefab;

	[SerializeField]
	private ActionController _ActionController;

	private ScrollRect _TimelineScrollRect;

	private List<TimelineActionWidget> _TimelineActionWidgets = new List<TimelineActionWidget>();

	private void Awake()
	{
		_TimelineScrollRect = GetComponent<ScrollRect>();

		_ActionController.ActionAdded += ActionAdded;
		_ActionController.ActionUpdated += ActionUpdated;
		_ActionController.ActionDeleted += ActionDeleted;
		_ActionController.ActionMoved += GetActionMoved;
	}

	private void ActionAdded(ActionData action)
	{
		TimelineActionWidget timelineAction = GameObject.Instantiate(_ButtonPrefab, _TimelineScrollRect.content).GetComponent<TimelineActionWidget>();
		timelineAction.gameObject.name = "TimelineActionWidget_" + _TimelineActionWidgets.Count;
		timelineAction.Setup(action, _ActionController);
		_TimelineActionWidgets.Add(timelineAction);


		// Make sure the UI is fully up to date to avoid glitching caused by the layout updating the next frame
		LayoutRebuilder.ForceRebuildLayoutImmediate(_TimelineScrollRect.content);
		Canvas.ForceUpdateCanvases();

		// Align the timeline with the last added timeline action
		_TimelineScrollRect.horizontalNormalizedPosition = 1;
	}

	private void ActionUpdated(ActionData action)
	{
		_TimelineActionWidgets[action.Index - 1].UpdateState();
	}

	private void ActionDeleted(ActionData action)
	{
		Destroy(_TimelineActionWidgets[action.Index - 1].gameObject);
		_TimelineActionWidgets.RemoveAt(action.Index - 1);
	}

	private void GetActionMoved(ActionData action, int newIndex)
	{
		TimelineActionWidget timeLineActionWidget = _TimelineActionWidgets[action.Index - 1];
		_TimelineActionWidgets.RemoveAt(action.Index - 1);
		if (newIndex > action.Index) newIndex--;
		_TimelineActionWidgets.Insert(newIndex - 1, timeLineActionWidget);
	}
}
