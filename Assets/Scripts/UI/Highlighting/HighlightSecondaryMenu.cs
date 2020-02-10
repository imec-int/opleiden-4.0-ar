using UnityEngine;
using UnityEngine.Events;
using Data;
using Core;

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
		#endregion

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}

		public void Setup(Operation[] operations, PartType partType, UnityAction infoButtonListener, ActionController controller, Highlight parent)
		{
			_actionController = controller;
			_highlightParent = parent;
			// operation buttons
			foreach (Operation op in operations)
			{
				ActionWidget elem = CreateNewActionElement();
				elem.Setup(op, partType);
				elem.AssociatedButton.onClick.AddListener(
					() => OnOperationButtonClicked(new IndexedActionData { Operation = op, PartType = partType, Part = _highlightParent.gameObject }));
			}

			// info button
			ActionWidget infoElem = CreateNewActionElement();
			infoElem.Setup("Info", "\uF2D7");
			infoElem.AssociatedButton.onClick.AddListener(infoButtonListener);
		}

		private ActionWidget CreateNewActionElement()
		{
			ActionWidget newObj = GameObject.Instantiate(_SecondaryButtonPrefab, _rectTransform);
			newObj.transform.SetParent(this.transform, false);
			return newObj;
		}

		private void OnOperationButtonClicked(IndexedActionData action)
		{
			// Debug.Log($"Clicked {action.Operation}, {action.Part}");
			_actionController.AddAction(action);
			_highlightParent.Collapse();
		}
	}
}
