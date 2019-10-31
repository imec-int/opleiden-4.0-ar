using System.Collections.Generic;
using UnityEngine;
using Data;
using Core;

namespace UI.Highlighting
{
	public class HighlightContainer : MonoBehaviour
	{
		[SerializeField]
		private GameObject _3DModel = null;

		[SerializeField]
		private Highlight _UIHighlightPrefab = null;

		[SerializeField]
		private InfoPanel _UIInfoPanel = null;

		[SerializeField]
		private ActionController _actionController = null;

		private List<Highlight> _uiHighlightInstanceList = new List<Highlight>();

		#region Monobehaviour
		private void OnEnable()
		{
			_3DModel = GameObject.Find("Installation");
			Reset();
		}
		#endregion

		#region Interface
		public void Reset(GameObject newmodel = null)
		{
			// Clean up
			foreach (Highlight highlight in _uiHighlightInstanceList)
			{
				highlight.OnExpanded -= OnHighlightSelected;
				Destroy(highlight.gameObject);
			}
			_uiHighlightInstanceList.Clear();

			if (newmodel)
			{
				PlaceHighlights(newmodel);
				_3DModel = newmodel;
			}
			else
				PlaceHighlights(_3DModel);
		}

		// Enables/Disables visibility of all the highlights
		public void SetHighlightsVisibility(bool visible)
		{
			gameObject.SetActive(visible);
		}
		#endregion

		#region Methods
		private void PlaceHighlights(GameObject model)
		{
			HighlightAnchor[] anchors = model.GetComponentsInChildren<HighlightAnchor>();
			Debug.Assert(anchors.Length > 0, $"Failed to retrieve anchors for the provided gameobject {model.name}");

			foreach (HighlightAnchor anchor in anchors)
			{
				Highlight newObj = Instantiate(_UIHighlightPrefab, anchor.transform.position, Quaternion.Euler(0, 0, 0), this.transform);
				newObj.Setup(anchor, OnInfoPanelRequested, _actionController);
				_uiHighlightInstanceList.Add(newObj);
				// Set up new object
				newObj.OnExpanded += OnHighlightSelected;
			}
		}
		#endregion

		#region Callbacks
		private void OnHighlightSelected(Highlight sender)
		{
			// Collapse all non-selected highlights
			foreach (Highlight highlight in _uiHighlightInstanceList)
			{
				if (highlight != sender)
				{
					highlight.Collapse();
				}
			}
		}

		private void OnInfoPanelRequested(HighlightInfo info)
		{
			SetHighlightsVisibility(false);
			_UIInfoPanel.Show(info);
			_UIInfoPanel.OnClose += OnInfoPanelClosed;
		}

		private void OnInfoPanelClosed()
		{
			SetHighlightsVisibility(true);
			_UIInfoPanel.OnClose -= OnInfoPanelClosed;
		}
		#endregion

	}
}
