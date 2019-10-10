using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateVariableHolder : MonoBehaviour
{
    [SerializeField]
    private GameObject _LoadingScreen;

    public GameObject LoadingScreen
    {
        get
        {
            return _LoadingScreen;
        }
    }
}
