using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARRaycastManager))]
public class ObjectPlacement : MonoBehaviour
{
	private ARRaycastManager _RaycastManager;
	private ARSessionOrigin _SessionOrigin;

	[SerializeField]
	private Transform _Installation;

	private void Awake()
	{
		_SessionOrigin = GetComponent<ARSessionOrigin>();
		_RaycastManager = GetComponent<ARRaycastManager>();
	}

	// Start is called before the first frame update
	void Update()
	{
		if (Input.touchCount == 0 || _Installation == null)
			return;

		List<ARRaycastHit> hits = new List<ARRaycastHit>();

		Vector3 pos = Camera.main.WorldToViewportPoint(_Installation.position);

		if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1 || pos.z < 0 || _Installation.gameObject.activeSelf == false)
		{
			if (_RaycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
			{
				_Installation.gameObject.SetActive(true);
				//_SessionOrigin.MakeContentAppearAt(_Installation, hits[0].pose.position);

				_Installation.position = hits[0].pose.position;
				_Installation.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			}
		}
	}

}
