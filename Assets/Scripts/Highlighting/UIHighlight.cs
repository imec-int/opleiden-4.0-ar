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
    private Button _Button = null;
    [SerializeField]
    private TextMeshProUGUI[] _ActionPositionLabels = new TextMeshProUGUI[4];

    private RectTransform _RectTransform = null;
    private Camera _MainCamera = null;

    // Boilerplate
    void Awake()
    {
        _Button.onClick.AddListener(onButtonClicked);
        _RectTransform = this.GetComponent<RectTransform>();
        _MainCamera = Camera.main;
    }

    void Update()
    {
        _RectTransform.LookAt(_MainCamera.transform);
    }
    
    private void onButtonClicked()
    {
        //Show the menu
    }
}