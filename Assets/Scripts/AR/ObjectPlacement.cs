using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace AR
{
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
		private void Update()
		{
			if (Input.touchCount == 0 || _installation == null)
				return;

			List<ARRaycastHit> hits = new List<ARRaycastHit>();

			Vector3 pos = Camera.main.WorldToViewportPoint(_installation.position);

			if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1 || pos.z < 0 || !_installation.gameObject.activeSelf)
			{
				if (Input.GetTouch(0).phase == TouchPhase.Began && _raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
				{
					_installation.gameObject.SetActive(true);
					_sessionOrigin.MakeContentAppearAt(_installation, hits[0].pose.position);

					transform.localScale = new Vector3(1 / _installationScale, 1 / _installationScale, 1 / _installationScale);
				}
			}
		}
	}
}
