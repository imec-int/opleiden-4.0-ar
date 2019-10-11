using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHighlightContainer : MonoBehaviour
{
    [SerializeField]
    private GameObject _3DModel = null;
    [SerializeField]
    private UIHighlight _UIHighlightPrefab = null;

    private List<UIHighlight> _UIHighlightInstanceList = new List<UIHighlight>();

    public void Reset(GameObject newmodel = null)
    {
        foreach(UIHighlight highlight in _UIHighlightInstanceList)
        {
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

    public void SetVisibility(bool visibile)
    {
        foreach(UIHighlight highlight in _UIHighlightInstanceList)
        {
            highlight.gameObject.SetActive(visibile);
        }
    }

    private void PlaceHighlights(GameObject model)
    {
        HighlightAnchor[] anchors = model.GetComponentsInChildren<HighlightAnchor>();
        Debug.Assert(anchors.Length > 0, $"Failed to retrieve anchors for the provided gameobject {model.name}");

        foreach(HighlightAnchor anchor in anchors)
        {
            UIHighlight newObj = GameObject.Instantiate(_UIHighlightPrefab, anchor.transform.position, Quaternion.Euler(0,0,0));
            // TODO: Additional setup from anchor           
            newObj.Setup(anchor);
            _UIHighlightInstanceList.Add(newObj);
            // Set up new object
            newObj.transform.SetParent(this.transform,true);
        }        
    }
}
