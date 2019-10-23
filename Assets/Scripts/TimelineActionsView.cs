using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TimeLineValidation;
public class TimelineActionsView : MonoBehaviour
{
	[SerializeField]
	private TimelineActionWidget _buttonPrefab;

	[SerializeField]
	private ActionController _actionController;

	[SerializeField]
	private float _followPerc = 0.25f;

	[SerializeField]
	private float _followSpeed = 0.5f;

	[SerializeField]
	private Button _btnTimelineValidation;

	private ScrollRect _timelineScrollRect;
	private Rect _timelineRect;

	private readonly List<TimelineActionWidget> _timelineActionWidgets = new List<TimelineActionWidget>();

	private void Awake()
	{
		_timelineScrollRect = GetComponent<ScrollRect>();

		//TODO [PLDN-55]: Recalculate rect when device rotation changes
		_timelineRect = GetComponent<RectTransform>().ToScreenSpace();

		_actionController.ActionAdded += ActionAdded;
		_actionController.ActionUpdated += ActionUpdated;
		_actionController.ActionDeleted += ActionDeleted;
		_actionController.ActionMoved += ActionMoved;

		_btnTimelineValidation.onClick.AddListener(() => _actionController.ValidateActions());
	}

	private void ActionAdded(IndexedActionData action)
	{
		TimelineActionWidget timelineAction = GameObject.Instantiate(_buttonPrefab, _timelineScrollRect.content).GetComponent<TimelineActionWidget>();
		timelineAction.gameObject.name = "TimelineActionWidget_" + _timelineActionWidgets.Count;
		timelineAction.Setup(action, _actionController);
		_timelineActionWidgets.Add(timelineAction);

		timelineAction.GetComponent<LongPressDragAndDrop>()._OnDrag.AddListener(OnWidgetDrag);

		// Make sure the UI is fully up to date to avoid glitching caused by the layout updating the next frame
		LayoutRebuilder.ForceRebuildLayoutImmediate(_timelineScrollRect.content);
		Canvas.ForceUpdateCanvases();

		// Align the timeline with the last added timeline action
		_timelineScrollRect.horizontalNormalizedPosition = 1;
	}

	private void ActionUpdated(IndexedActionData action)
	{
		_timelineActionWidgets[action.Index - 1].UpdateState();
	}

	private void ActionDeleted(IndexedActionData action)
	{
		Destroy(_timelineActionWidgets[action.Index - 1].gameObject);
		_timelineActionWidgets.RemoveAt(action.Index - 1);
	}

	private void ActionMoved(IndexedActionData action, int newIndex)
	{
		TimelineActionWidget timeLineActionWidget = _timelineActionWidgets[action.Index - 1];
		_timelineActionWidgets.RemoveAt(action.Index - 1);
		_timelineActionWidgets.Insert(newIndex - 1, timeLineActionWidget);
	}

	public void OnWidgetDrag(PointerEventData eventData)
	{
		if (eventData.position.x < _timelineRect.center.x)
		{
			float speedLerp = Mathf.Clamp01(eventData.position.x.RemapValue(_timelineRect.xMin + _timelineRect.width * _followPerc, _timelineRect.xMin, 0, 1));
			_timelineScrollRect.horizontalNormalizedPosition = Mathf.Clamp01(_timelineScrollRect.horizontalNormalizedPosition - _followSpeed * speedLerp * Time.deltaTime);
		}
		else
		{
			float speedLerp = Mathf.Clamp01(eventData.position.x.RemapValue(_timelineRect.center.x + _timelineRect.width * _followPerc, _timelineRect.xMax, 0, 1));
			_timelineScrollRect.horizontalNormalizedPosition = Mathf.Clamp01(_timelineScrollRect.horizontalNormalizedPosition + _followSpeed * speedLerp * Time.deltaTime);
		}
	}
}
