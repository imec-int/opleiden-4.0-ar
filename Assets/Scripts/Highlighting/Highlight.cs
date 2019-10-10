using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour
{
    // Unity-facing variables
    // Useful children
    [SerializeField]
    private Image _BGImage;
    [SerializeField]
    private Button _Button;
    [SerializeField]
    private TextMeshProUGUI[] _PositionLabels = new TextMeshProUGUI[4];

    // Code interface
    public Vector3 AssociatedWSPoint
    {
        get;
        set;
    }

    private uint _UID;
    public uint UID
    {
        get
        {
            return _UID;
        }
    }

    // Boilerplate
    void Awake()
    {
        _Button.onClick.AddListener(onButtonClicked);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // The meat
    //=============
    // Update is called once per frame
    void Update()
    {
        // Track 3D Position

    }

    private void onButtonClicked()
    {
        // Spawn the actions
    }
}
