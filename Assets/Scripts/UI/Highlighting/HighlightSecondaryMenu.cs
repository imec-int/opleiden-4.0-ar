using UnityEngine;
using UnityEngine.Events;
using Data;
using Core;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UI.Highlighting
{
	public class HighlightSecondaryMenu : MonoBehaviour
	{
		[SerializeField]
		private ActionWidget _SecondaryButtonPrefab = null;

		#region Members
		private RectTransform _rectTransform = null;
		private HighlightInfo _highlightInfo = null;
		private ActionController _actionController = null;
		private Highlight _highlightParent = null;
		private Dictionary<IndexedActionData, ActionWidget> _actionWidgetDictionary = new Dictionary<IndexedActionData, ActionWidget>();
		private ActionWidget _infoActionWidget = null;
		#endregion

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}

		protected void OnDestroy()
		{
			_actionController.ValidationCompleted -= HandleConsequences;
		}

		public void Setup(Operation[] operations, PartType partType, UnityAction infoButtonListener, ActionController controller, Highlight parent)
		{
			_actionController = controller;
			_actionController.ValidationCompleted += HandleConsequences;
			_actionController.ActionDeleted += ReenableAction;
			_highlightParent = parent;
			// operation buttons
			foreach (Operation op in operations)
			{
				IndexedActionData indexedActionData = new IndexedActionData { Operation = op, PartType = partType, Part = _highlightParent.gameObject };
				ActionWidget elem = CreateNewActionElement();
				_actionWidgetDictionary.Add(indexedActionData, elem);
				elem.Setup(op, partType);
				elem.AssociatedButton.onClick.AddListener(
					() => OnOperationButtonClicked(elem.AssociatedButton, indexedActionData));
			}

			// info button
			_infoActionWidget = CreateNewActionElement();
			_infoActionWidget.Setup("Info", "\uF2D7");
			_infoActionWidget.AssociatedButton.onClick.AddListener(infoButtonListener);
		}

		private ActionWidget CreateNewActionElement()
		{
			ActionWidget newObj = GameObject.Instantiate(_SecondaryButtonPrefab, _rectTransform);
			newObj.transform.SetParent(this.transform, false);
			return newObj;
		}

		private void OnOperationButtonClicked(Button button, IndexedActionData action)
		{
			// Debug.Log($"Clicked {action.Operation}, {action.Part}");
			_actionController.AddAction(action);
			_highlightParent.Collapse();
			button.interactable = false;
		}

		private void ReenableAction(IndexedActionData obj)
		{
			if (_actionWidgetDictionary.TryGetValue(obj, out ActionWidget widget))
			{
				widget.AssociatedButton.interactable = true;
			}
		}

		private void HandleConsequences(ValidationStageReport report)
		{
			if (_highlightParent.AssociatedAnchor.Consequences.Exists(data => !string.IsNullOrEmpty(data.Body)))
			{
				_infoActionWidget.SetIcon("\uF026");
			}
		}
	}
}
