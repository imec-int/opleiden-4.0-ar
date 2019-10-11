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

    [SerializeField]
    private float minSize = 0.1f;
    [SerializeField]
    private float maxSize = 1.0f;

    private RectTransform _RectTransform = null;

    public bool DebugToggleLookat = false;

    // Boilerplate
    void Awake()
    {
        _Button.onClick.AddListener(onButtonClicked);
        _RectTransform = this.GetComponent<RectTransform>();
        _RectTransform.localScale = Vector3.one*Mathf.Lerp(maxSize,minSize,GetRelativeDepth());
    }

    void Update()
    {
        _RectTransform.localScale = Vector3.one*Mathf.Lerp(maxSize,minSize,GetRelativeDepth());
        _RectTransform.rotation = Matrix4x4.LookAt(this.transform.position, Camera.main.transform.position, Vector3.up).rotation;
    }

    private float GetRelativeDepth()
    {
        // Dynamic scaling
        var cameraSpacePos = Camera.main.worldToCameraMatrix*this.transform.position;
        return cameraSpacePos.z.RemapValue(Camera.main.nearClipPlane, Camera.main.farClipPlane*-1,0,1);
    }
    private void onButtonClicked()
    {
        //Show the menu
    }
}