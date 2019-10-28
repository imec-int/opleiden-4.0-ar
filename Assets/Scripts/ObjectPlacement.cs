using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARRaycastManager))]
public class ObjectPlacement : MonoBehaviour
{
	private ARRaycastManager _raycastManager;
	private ARSessionOrigin _sessionOrigin;

	[SerializeField]
	private Transform _installation;

	[SerializeField]
	private float _installationScale = 1;

	private void Awake()
	{
		_sessionOrigin = GetComponent<ARSessionOrigin>();
		_raycastManager = GetComponent<ARRaycastManager>();
	}

	// Start is called before the first frame update
	void Update()
	{
		if (Input.touchCount == 0 || _installation == null)
			return;

		List<ARRaycastHit> hits = new List<ARRaycastHit>();

		Vector3 pos = Camera.main.WorldToViewportPoint(_installation.position);

		if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1 || pos.z < 0 || _installation.gameObject.activeSelf == false)
		{
			if (_raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
			{
				_installation.gameObject.SetActive(true);
				//_SessionOrigin.MakeContentAppearAt(_Installation, hits[0].pose.position);

				_installation.position = hits[0].pose.position;
				_installation.localScale = new Vector3(_installationScale, _installationScale, _installationScale);
			}
		}
	}

}
