using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHighlightContainer : MonoBehaviour
{
    [SerializeField]
    private GameObject _3DModel = null;
    [SerializeField]
    private UIHighlight _UIHighlightPrefab = null;

    private List<UIHighlight> _UIHighlightInstancesList = new List<UIHighlight>();

    // Start is called before the first frame update
    void Start()
    {
        PlaceHighlights(_3DModel);
        // debug
        //Reset(_3DModel);
    }

    public void Reset(GameObject newmodel = null)
    {
        foreach(var highlight in _UIHighlightInstancesList)
        {
            highlight.StopAllCoroutines();
            GameObject.Destroy(highlight.gameObject);
        }
        _UIHighlightInstancesList.Clear();

        if (newmodel)
            PlaceHighlights(newmodel);
    }

    public void SetVisibility(bool visibile)
    {
        foreach(var highlight in _UIHighlightInstancesList)
        {
            highlight.gameObject.SetActive(visibile);
        }
    }

    private void PlaceHighlights(GameObject model)
    {
        var anchors = model.GetComponentsInChildren<HighlightAnchor>();
        Debug.Assert(anchors.Length > 0, $"Failed to retrieve anchors for the provided gameobject {model.name}");

        foreach(var anchor in anchors)
        {
            var newObj = GameObject.Instantiate(_UIHighlightPrefab, anchor.transform.position, Quaternion.Euler(0,0,0));
            // TODO: Additional setup from anchor           
            _UIHighlightInstancesList.Add(newObj);
            // Set up new object
            newObj.transform.SetParent(this.transform,true);
        }        
    }
}
