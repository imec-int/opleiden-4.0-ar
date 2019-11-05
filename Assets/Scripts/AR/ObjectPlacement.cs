using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace AR
{
	[RequireComponent(typeof(ARSessionOrigin), typeof(ARRaycastManager))]
	public class ObjectPlacement : MonoBehaviour
	{
		public UnityEvent ObjectPlaced;

		private ARRaycastManager _raycastManager;
		private ARSessionOrigin _sessionOrigin;

		[SerializeField]
		private GameObject _installationPrefab;

		private Transform _installation;

		[SerializeField]
		private float _installationScale = 1;

		protected void Awake()
		{
			_sessionOrigin = GetComponent<ARSessionOrigin>();
			_raycastManager = GetComponent<ARRaycastManager>();
		}

		protected void Update()
		{
			if (Input.touchCount == 0)
				return;

			List<ARRaycastHit> hits = new List<ARRaycastHit>();

			Vector3 pos = Vector3.zero;

			if (_installation == null)
			{
				pos = Camera.main.WorldToViewportPoint(_installation.position);
			}

			if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1 || pos.z < 0 || _installation == null)
			{
				if ((Input.GetTouch(0).tapCount == 2 || _installation == null) && Input.GetTouch(0).phase == TouchPhase.Began && _raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
				{
					if (!_installation.gameObject.activeSelf) ObjectPlaced.Invoke();
					_installation = Instantiate(_installationPrefab).transform;
					_sessionOrigin.MakeContentAppearAt(_installation, hits[0].pose.position);

					transform.localScale = new Vector3(1 / _installationScale, 1 / _installationScale, 1 / _installationScale);
				}
			}
		}
	}
}
