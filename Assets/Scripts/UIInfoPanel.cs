using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIInfoPanel : MonoBehaviour
{
    // TODO: need to refactor our own action class to make this easier
    public event System.Action OnOpen;
    public event System.Action OnClose;

    [SerializeField]
    private TextMeshProUGUI _HeaderLabel;
    [SerializeField]
    private TextMeshProUGUI _BodyLabel;
    [SerializeField]
    private Button _CloseButton;

    public void Awake()
    {
        _CloseButton.onClick.AddListener(Close);
    }   

    public void Show(HighlightInfo info)
    {
        OnOpen?.Invoke();
        _HeaderLabel.text = info.Header;
        _BodyLabel.text = info.Body;

        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        OnClose?.Invoke();
        this.gameObject.SetActive(false);
    }
}
