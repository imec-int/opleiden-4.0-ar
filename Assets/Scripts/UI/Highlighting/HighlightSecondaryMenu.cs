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
		private RectTransform _RectTransform = null;
		private HighlightInfo _HighlightInfo = null;
		private ActionController _ActionController = null;
		private Highlight _HighlightParent = null;
		#endregion

		private void Awake()
		{
			_RectTransform = GetComponent<RectTransform>();
		}

		public void Setup(Operation[] operations, Part part, UnityAction infoButtonListener, ActionController controller, Highlight parent)
		{
			_ActionController = controller;
			_HighlightParent = parent;
			// operation buttons
			foreach (Operation op in operations)
			{
				ActionWidget elem = CreateNewActionElement();
				elem.Setup(op, part);
				elem.AssociatedButton.onClick.AddListener(
					() => OnOperationButtonClicked(new IndexedActionData { Operation = op, Part = part }));
			}

			// info button
			ActionWidget infoElem = CreateNewActionElement();
			infoElem.Setup("Info", "\uE887");
			infoElem.AssociatedButton.onClick.AddListener(infoButtonListener);
		}

		private ActionWidget CreateNewActionElement()
		{
			ActionWidget newObj = GameObject.Instantiate(_SecondaryButtonPrefab, _RectTransform);
			newObj.transform.SetParent(this.transform, false);
			return newObj;
		}

		private void OnOperationButtonClicked(IndexedActionData action)
		{
			// Debug.Log($"Clicked {action.Operation}, {action.Part}");
			_ActionController.AddAction(action);
			_HighlightParent.Collapse();
		}
	}
}
