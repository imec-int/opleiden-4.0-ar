using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Data;
using UI.Utilities;
using Utilities;
using Core;
using System;
using TMPro;

namespace UI
{
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

		[SerializeField]
		private TextMeshProUGUI _lblButtonText;

		[SerializeField]
		private ActionToAnchorHighlighter _actionToAnchorHighlighter;

		private const string _validationButtonText = "Valideer Inspectie";
		private const string _validationButtonRetryText = "Valideer opnieuw";
		private const string _infoRibbonText = "Selecteer alle acties die nodig zijn bij inspectie van de pomp voor opstart";
		private const string _infoRibbonRetryText = "Klik op de rode highlights voor meer info over je fouten<br>Probeer het daarna opnieuw";

		private ScrollRect _timelineScrollRect;
		private Rect _timelineRect;

		private readonly List<TimelineActionWidget> _timelineActionWidgets = new List<TimelineActionWidget>();

		private void Awake()
		{
			_timelineScrollRect = GetComponent<ScrollRect>();

			_actionController.ActionAdded += ActionAdded;
			_actionController.ActionUpdated += ActionUpdated;
			_actionController.ActionDeleted += ActionDeleted;
			_actionController.ActionMoved += ActionMoved;
			_actionController.PostReset += ActionsReset;

			_btnTimelineValidation.onClick.AddListener(() => _actionController.ValidateActions());
			_btnTimelineValidation.onClick.AddListener(PostValidationLabelUpdate);

			IndexedActionData pumpTagAction = new IndexedActionData();
			pumpTagAction.PartType = PartType.Pump;
			pumpTagAction.Operation = Operation.Label;
			ActionAdded(pumpTagAction, true, false);
		}

		private void PostValidationLabelUpdate()
		{
			TopInfoRibbon.Instance.SetLabelText(_infoRibbonRetryText);
			_lblButtonText.text = _validationButtonRetryText;
		}

		private void ActionsReset()
		{
			foreach (TimelineActionWidget widget in _timelineActionWidgets)
			{
				Destroy(widget.gameObject);
			}
			_timelineActionWidgets.Clear();
			_lblButtonText.text = _validationButtonText;
		}

		private void OnEnable()
		{
			OnRectTransformChanged();
			TopInfoRibbon.Instance?.SetLabelText(_infoRibbonText);
		}

		public void OnRectTransformChanged()
		{
			if (gameObject.activeInHierarchy)
			{
				StartCoroutine(UpdateTransforms());
			}
		}

		private IEnumerator UpdateTransforms()
		{
			yield return 0;
			_timelineRect = GetComponent<RectTransform>().ToScreenSpace();
		}

		private void ActionAdded(IndexedActionData action)
		{
			ActionAdded(action, true, true);
		}

		private void ActionAdded(IndexedActionData action, bool staticAction = false, bool includeInValidation = true)
		{
			TimelineActionWidget timelineAction = GameObject.Instantiate(_buttonPrefab, _timelineScrollRect.content).GetComponent<TimelineActionWidget>();
			timelineAction.gameObject.name = "TimelineActionWidget_" + _timelineActionWidgets.Count;
			timelineAction.Setup(action, _actionController, includeInValidation);
			if (includeInValidation)
			{
				// Timeline actions that are included in validation should be removable
				timelineAction.SetDeleteBtnActive(true);
				_timelineActionWidgets.Add(timelineAction);
				timelineAction.OnClick += LinkActionToHiglight;
			}
			if (!staticAction) timelineAction.GetComponent<LongPressDragAndDrop>()._OnDrag.AddListener(OnWidgetDrag);

			// Make sure the UI is fully up to date to avoid glitching caused by the layout updating the next frame
			LayoutRebuilder.ForceRebuildLayoutImmediate(_timelineScrollRect.content);
			Canvas.ForceUpdateCanvases();

			// Align the timeline with the last added timeline action
			_timelineScrollRect.horizontalNormalizedPosition = 1;
		}

		private void LinkActionToHiglight(object sender, EventArgs e)
		{
			var widget = sender as TimelineActionWidget;
			// Show the arrow
			_actionToAnchorHighlighter.ShowArrow(widget, widget.AssociatedActionData.Part);
		}

		private void ActionUpdated(IndexedActionData action)
		{
			_timelineActionWidgets[action.Index - 1].UpdateState();
		}

		private void ActionDeleted(IndexedActionData action)
		{
			_timelineActionWidgets[action.Index - 1].OnClick -= LinkActionToHiglight;
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
				float speedLerp = Mathf.Clamp01(eventData.position.x.RemapValue(_timelineRect.xMin + (_timelineRect.width * _followPerc), _timelineRect.xMin, 0, 1));
				_timelineScrollRect.horizontalNormalizedPosition = Mathf.Clamp01(_timelineScrollRect.horizontalNormalizedPosition - (_followSpeed * speedLerp * Time.deltaTime));
			}
			else
			{
				float speedLerp = Mathf.Clamp01(eventData.position.x.RemapValue(_timelineRect.center.x + (_timelineRect.width * _followPerc), _timelineRect.xMax, 0, 1));
				_timelineScrollRect.horizontalNormalizedPosition = Mathf.Clamp01(_timelineScrollRect.horizontalNormalizedPosition + (_followSpeed * speedLerp * Time.deltaTime));
			}
		}
	}
}
