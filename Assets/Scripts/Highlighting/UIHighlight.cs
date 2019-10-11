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

    public HighlightAnchor AssociatedAnchor
    {
        get;private set;
    }

    public void Setup(HighlightAnchor anchor)
    {
        AssociatedAnchor = anchor;
        _SecondaryMenu.Setup(anchor.AvailableOperations,anchor.HighlightedPart);
        _SecondaryMenu.gameObject.SetActive(false);
    }

    // Boilerplate
    void Awake()
    {
        _MainButton.onClick.AddListener(onButtonClicked);
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
        //Show the menu
        _SecondaryMenu.gameObject.SetActive(true);
    }
}