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
    
    // Boilerplate
    void Awake()
    {
        _Button.onClick.AddListener(onButtonClicked);
        _RectTransform = this.GetComponent<RectTransform>();
    }

    void Update()
    {
        var cameraSpacePos = Camera.main.worldToCameraMatrix*this.transform.position;
        var relativeDepth = cameraSpacePos.z.RemapValue(Camera.main.nearClipPlane, Camera.main.farClipPlane*-1,0,1);
        _RectTransform.localScale = Vector3.one*Mathf.Lerp(maxSize,minSize,relativeDepth);
    }

    private void onButtonClicked()
    {
        //Show the menu
    }
}