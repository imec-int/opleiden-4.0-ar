using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public event EventHandler OnSelected;

    public HighlightAnchor AssociatedAnchor
    {
        get;private set;
    }
    public bool Selected
    {
        get; private set;
    }

    public void Setup(HighlightAnchor anchor)
    {
        AssociatedAnchor = anchor;
        _SecondaryMenu.Setup(anchor.AvailableOperations,anchor.HighlightedPart);
        _SecondaryMenu.gameObject.SetActive(false);
        _MainButton.onClick.AddListener(onButtonClicked);
    }

    public void Collapse()
    {
        _SecondaryMenu.gameObject.SetActive(false);        
        Selected = false;
        // TODO: Color change
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
    
    private void onButtonClicked()
    {
        Expand();
        OnSelected?.Invoke(this, new EventArgs());
    }

    private void Expand()
    {
        //Show the menu
        _SecondaryMenu.gameObject.SetActive(true);        
        Selected = true;
        // TODO: Color change
    }
}