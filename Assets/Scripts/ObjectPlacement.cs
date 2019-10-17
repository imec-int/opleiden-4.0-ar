using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARRaycastManager))]
public class ObjectPlacement : MonoBehaviour
{
	[SerializeField]
	private ARRaycastManager _RaycastManager;

	[SerializeField]
	private ARSessionOrigin _SessionOrigin;

	[SerializeField]
	private Transform _Installation;

	// Start is called before the first frame update
	void Update()
	{
		if (Input.touchCount == 0 || _Installation == null)
			return;

		List<ARRaycastHit> hits = new List<ARRaycastHit>();


		if (_RaycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
		{
			_Installation.gameObject.SetActive(true);
			//_SessionOrigin.MakeContentAppearAt(_Installation, hits[0].pose.position);

			_Installation.position = hits[0].pose.position;
		}
	}

}
