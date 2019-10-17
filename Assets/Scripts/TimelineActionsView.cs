using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimelineActionsView : MonoBehaviour
{
	[SerializeField]
	private TimelineActionWidget _ButtonPrefab;

	[SerializeField]
	private ActionController _ActionController;

	[SerializeField]
	private float _FollowPerc = 0.25f;

	[SerializeField]
	private float _FollowSpeed = 0.5f;

	private ScrollRect _TimelineScrollRect;
	private Rect _TimelineRect;

	private List<TimelineActionWidget> _TimelineActionWidgets = new List<TimelineActionWidget>();

	private void Awake()
	{
		_TimelineScrollRect = GetComponent<ScrollRect>();

		//TODO [PLDN-55]: Recalculate rect when device rotation changes
		_TimelineRect = GetComponent<RectTransform>().ToScreenSpace();

		_ActionController.ActionAdded += ActionAdded;
		_ActionController.ActionUpdated += ActionUpdated;
		_ActionController.ActionDeleted += ActionDeleted;
		_ActionController.ActionMoved += ActionMoved;
	}

	private void ActionAdded(ActionData action)
	{
		TimelineActionWidget timelineAction = GameObject.Instantiate(_ButtonPrefab, _TimelineScrollRect.content).GetComponent<TimelineActionWidget>();
		timelineAction.gameObject.name = "TimelineActionWidget_" + _TimelineActionWidgets.Count;
		timelineAction.Setup(action, _ActionController);
		_TimelineActionWidgets.Add(timelineAction);

		timelineAction.GetComponent<LongPressDragAndDrop>()._OnDrag.AddListener(OnWidgetDrag);


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

	private void ActionMoved(ActionData action, int newIndex)
	{
		TimelineActionWidget timeLineActionWidget = _TimelineActionWidgets[action.Index - 1];
		_TimelineActionWidgets.RemoveAt(action.Index - 1);
		_TimelineActionWidgets.Insert(newIndex - 1, timeLineActionWidget);
	}

	public void OnWidgetDrag(PointerEventData eventData)
	{
		if (eventData.position.x < _TimelineRect.center.x)
		{
			float speedLerp = Mathf.Clamp01(eventData.position.x.RemapValue(_TimelineRect.xMin + _TimelineRect.width * _FollowPerc, _TimelineRect.xMin, 0, 1));
			_TimelineScrollRect.horizontalNormalizedPosition = Mathf.Clamp01(_TimelineScrollRect.horizontalNormalizedPosition - _FollowSpeed * speedLerp * Time.deltaTime);
		}
		else
		{
			float speedLerp = Mathf.Clamp01(eventData.position.x.RemapValue(_TimelineRect.center.x + _TimelineRect.width * _FollowPerc, _TimelineRect.xMax, 0, 1));
			_TimelineScrollRect.horizontalNormalizedPosition = Mathf.Clamp01(_TimelineScrollRect.horizontalNormalizedPosition + _FollowSpeed * speedLerp * Time.deltaTime);
		}
	}
}
