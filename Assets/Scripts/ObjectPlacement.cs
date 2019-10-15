using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectPlacement : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager _RaycastManager;
    [SerializeField]
    GameObject _Pump;
    

    // Start is called before the first frame update
    void Update()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        bool success = _RaycastManager.Raycast(Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)),hits,TrackableType.Planes);
        if (success)
        {
            _Pump.transform.position = hits[0].pose.position;
        }
    }

}
