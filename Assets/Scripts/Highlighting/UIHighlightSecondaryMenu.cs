using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHighlightSecondaryMenu : MonoBehaviour
{
    [SerializeField]
    private ActionElement _SecondaryButtonPrefab = null;

    private RectTransform _RectTransform = null;

    private void Awake()
	{
		_RectTransform = GetComponent<RectTransform>();
	}


    public void Setup(Operation[] operations, Part part, Action infocallback = null, Action actioncallback = null)
    {
        // operation buttons
        foreach(Operation op in operations)
        {
            ActionElement elem = CreateNewActionElement();
            elem.Setup(op,part);
        }
        // info button
        ActionElement infoElem = CreateNewActionElement();
        infoElem.Setup("Info","\uE887");
    }

    private ActionElement CreateNewActionElement()
    {
        ActionElement newObj = GameObject.Instantiate(_SecondaryButtonPrefab, _RectTransform);
        newObj.transform.SetParent(this.transform,false);
        return newObj;
    }
}
