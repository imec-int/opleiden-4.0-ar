using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIHighlightSecondaryMenu : MonoBehaviour
{
    [SerializeField]
    private UIActionElement _SecondaryButtonPrefab = null;
    private RectTransform _RectTransform = null;
    private HighlightInfo _HighlightInfo = null;
    [SerializeField]
    private ActionController _ActionController = null;
    private UIHighlight _HighlightParent = null;

    private void Awake()
	{
		_RectTransform = GetComponent<RectTransform>();
	}

    public void Setup(Operation[] operations, Part part, UnityAction infoButtonListener, ActionController controller, UIHighlight parent)
    {
        _ActionController = controller;
        _HighlightParent = parent;
        // operation buttons
        foreach(Operation op in operations)
        {
            UIActionElement elem = CreateNewActionElement();
            elem.Setup(op,part);
            elem.AssociatedButton.onClick.AddListener(
                ()=>
                {
                    OnOperationButtonClicked(new ActionData{Operation=op, Part=part});
                });
        }

        // info button
        UIActionElement infoElem = CreateNewActionElement();
        infoElem.Setup("Info","\uE887");
        infoElem.AssociatedButton.onClick.AddListener(infoButtonListener);
    }

    private UIActionElement CreateNewActionElement()
    {
        UIActionElement newObj = GameObject.Instantiate(_SecondaryButtonPrefab, _RectTransform);
        newObj.transform.SetParent(this.transform,false);
        return newObj;
    }

    private void OnOperationButtonClicked(ActionData action)
    {
        Debug.Log($"Clicked {action.Operation}, {action.Part}");
        // TODO: Add to action manager
        _ActionController.AddAction(action);
        _HighlightParent.Collapse();
    }
}
