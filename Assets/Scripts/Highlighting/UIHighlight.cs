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
    private Image _BGImage = null;
    [SerializeField]
    private Button _Button = null;
    [SerializeField]
    private TextMeshProUGUI[] _ActionPositionLabels = new TextMeshProUGUI[4];

    [SerializeField]
    private float minSize = 0.1f;
    [SerializeField]
    private float maxSize = 1.0f;

    private RectTransform _RectTransform = null;

    public Vector3 DEBUGPos = Vector3.zero;


    // Boilerplate
    void Awake()
    {
        _Button.onClick.AddListener(onButtonClicked);
        _RectTransform = this.GetComponent<RectTransform>();
    }

    void Update()
    {
        var cameraSpacePos = Camera.main.worldToCameraMatrix*this.transform.position;
        DEBUGPos = cameraSpacePos;
        _RectTransform.localScale = Vector3.one*Mathf.Lerp(maxSize,minSize,cameraSpacePos.z);
    }

    private void onButtonClicked()
    {
        //Show the menu
    }
}