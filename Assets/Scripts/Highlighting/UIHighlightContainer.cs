using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UI;

public class UIHighlightContainer : MonoBehaviour
{
	[SerializeField]
	private GameObject _3DModel = null;
	[SerializeField]
	private UIHighlight _UIHighlightPrefab = null;
	[SerializeField]
	private InfoPanel _UIInfoPanel = null;
	[SerializeField]
	private ActionController _actionController = null;

	private List<UIHighlight> _UIHighlightInstanceList = new List<UIHighlight>();

	#region Monobehaviour
	private void OnEnable()
	{
		Reset();
	}
	#endregion

	#region Interface
	public void Reset(GameObject newmodel = null)
	{
		// Clean up
		foreach (UIHighlight highlight in _UIHighlightInstanceList)
		{
			highlight.OnExpanded -= OnHighlightSelected;
			Destroy(highlight.gameObject);
		}
		_UIHighlightInstanceList.Clear();

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
			UIHighlight newObj = Instantiate(_UIHighlightPrefab, anchor.transform.position, Quaternion.Euler(0, 0, 0));
			newObj.Setup(anchor, OnInfoPanelRequested, _actionController);
			_UIHighlightInstanceList.Add(newObj);
			// Set up new object
			newObj.transform.SetParent(transform, true);
			newObj.OnExpanded += OnHighlightSelected;
		}
	}
	#endregion

	#region Callbacks
	private void OnHighlightSelected(UIHighlight sender)
	{
		// Collapse all non-selected highlights
		foreach (UIHighlight highlight in _UIHighlightInstanceList)
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
