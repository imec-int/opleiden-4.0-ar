using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHighlightSecondaryMenu : MonoBehaviour
{
    [SerializeField]
    private UIActionElement _SecondaryButtonPrefab = null;

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
            UIActionElement elem = CreateNewActionElement();
            elem.Setup(op,part);
        }
        // info button
        UIActionElement infoElem = CreateNewActionElement();
        infoElem.Setup("Info","\uE887");
    }

    private UIActionElement CreateNewActionElement()
    {
        UIActionElement newObj = GameObject.Instantiate(_SecondaryButtonPrefab, _RectTransform);
        newObj.transform.SetParent(this.transform,false);
        return newObj;
    }
}
