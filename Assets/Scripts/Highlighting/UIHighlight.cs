using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIHighlight : MonoBehaviour
{
    // Unity-facing variables
    // Useful children
    [SerializeField]
    private Button _MainButton = null;
    [SerializeField]
    private UIHighlightSecondaryMenu _SecondaryMenu = null;
    [SerializeField]
    private TextMeshProUGUI[] _ActionPositionLabels = new TextMeshProUGUI[4];

    private RectTransform _RectTransform = null;
    private Camera _MainCamera = null;

    public event System.Action<UIHighlight> OnExpanded;

    public HighlightAnchor AssociatedAnchor
    {
        get;private set;
    }
    public bool Selected
    {
        get; private set;
    }


    public void Setup(HighlightAnchor anchor, Action<HighlightInfo> showHighlightInfo, ActionController controller)
    {
        AssociatedAnchor = anchor;
        _SecondaryMenu.Setup(anchor.AvailableOperations,anchor.HighlightedPart,
        () => {
            showHighlightInfo?.Invoke(anchor.Info);
        },controller, this);

        _SecondaryMenu.gameObject.SetActive(false);
        _MainButton.onClick.AddListener(OnButtonClicked);
    }

    public void Collapse()
    {
        _SecondaryMenu.gameObject.SetActive(false);        
        Selected = false;
        // TODO: [PLDN-51] Color change
    }

    void Awake()
    {        
        _RectTransform = this.GetComponent<RectTransform>();
        _MainCamera = Camera.main;
    }

    void Update()
    {
        _RectTransform.LookAt(_MainCamera.transform);
        _RectTransform.forward *=-1;
    }
    
    // Event for the main button on this highlight
    private void OnButtonClicked()
    {
        Expand();
        OnExpanded?.Invoke(this);
    }

    private void Expand()
    {
        //Show the menu
        _SecondaryMenu.gameObject.SetActive(true);        
        Selected = true;
        // TODO: Color change
    }
}