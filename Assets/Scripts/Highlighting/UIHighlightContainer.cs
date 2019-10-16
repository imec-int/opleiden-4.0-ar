using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIHighlightContainer : MonoBehaviour
{
    [SerializeField]
    private GameObject _3DModel = null;
    [SerializeField]
    private UIHighlight _UIHighlightPrefab = null;
    [SerializeField]
    private UIInfoPanel _UIInfoPanel = null;
    [SerializeField]
    private ActionController _ActionController = null;

    private List<UIHighlight> _UIHighlightInstanceList = new List<UIHighlight>();

#region Monobehaviour
    // Monobehaviour
    //====================
    void Awake()
    {
        _UIInfoPanel.OnClose += OnInfoPanelClosed;
    }
#endregion

#region Interface
    public void Reset(GameObject newmodel = null)
    {
        // Clean up
        foreach(UIHighlight highlight in _UIHighlightInstanceList)
        {
            highlight.OnExpanded -= onHighlightSelected;
            GameObject.Destroy(highlight.gameObject);
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
        foreach(UIHighlight highlight in _UIHighlightInstanceList)
        {
            highlight.gameObject.SetActive(visible);
        }
    }
#endregion

#region Methods
    private void PlaceHighlights(GameObject model)
    {
        HighlightAnchor[] anchors = model.GetComponentsInChildren<HighlightAnchor>();
        Debug.Assert(anchors.Length > 0, $"Failed to retrieve anchors for the provided gameobject {model.name}");

        foreach(HighlightAnchor anchor in anchors)
        {
            UIHighlight newObj = GameObject.Instantiate(_UIHighlightPrefab, anchor.transform.position, Quaternion.Euler(0,0,0));  
            newObj.Setup(anchor, OnInfoPanelRequested, _ActionController);
            _UIHighlightInstanceList.Add(newObj);
            // Set up new object
            newObj.transform.SetParent(this.transform,true);
            newObj.OnExpanded += this.onHighlightSelected;
        }        
    }
#endregion

#region Callbacks
    private void onHighlightSelected(UIHighlight sender)
    {
        // Collapse all non-selected highlights
        foreach(UIHighlight highlight in _UIHighlightInstanceList)
        {
            if(highlight != sender)
            {
                highlight.Collapse();
            }
        }
    }

    private void OnInfoPanelRequested(HighlightInfo info)
    {
        SetHighlightsVisibility(false);
        _UIInfoPanel.Show(info);
    }

    private void OnInfoPanelClosed()
    {
        // We assume this can only be called when the highlights were already visible
        SetHighlightsVisibility(true);
    }
#endregion

}
