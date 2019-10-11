using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateVariableHolder : MonoBehaviour
{
    [SerializeField]
    private GameObject _LoadingScreen;
    [SerializeField]
    private GameObject _TimeLine;

    public GameObject LoadingScreen
    {
        get
        {
            return _LoadingScreen;
        }
    }

    public GameObject TimeLine
    {
        get
        {
            return _TimeLine;
        }
    }
}
