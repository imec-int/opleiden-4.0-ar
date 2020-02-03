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
		private ARPlaneManager _planeManager;
		private ARPointCloudManager _pointCloudManager;

		[SerializeField]
		private GameObject _installationPrefab;

		private Transform _installation;

		[SerializeField]
		private float _arScale = 1;

		private bool _isPumpPlaced;

		protected void Awake()
		{
			_sessionOrigin = GetComponent<ARSessionOrigin>();
			_raycastManager = GetComponent<ARRaycastManager>();

			_planeManager = GetComponent<ARPlaneManager>();
			_pointCloudManager = GetComponent<ARPointCloudManager>();
		}

		protected void Update()
		{
			if (Input.touchCount == 0)
				return;

			List<ARRaycastHit> hits = new List<ARRaycastHit>();

			if (Input.touchCount == 1 && _raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
			{
				if (!_isPumpPlaced)
					PlacePump(hits);
			}
		}

		public bool Reset()
		{
			if(!_installation) return false;
			Destroy(_installation.gameObject);
			_isPumpPlaced = false;
			return true;
		}

		private void PlacePump(List<ARRaycastHit> hits)
		{
			_installation = Instantiate(_installationPrefab).transform;
			_sessionOrigin.transform.localScale = new Vector3(_arScale, _arScale, _arScale);
			_sessionOrigin.MakeContentAppearAt(_installation, hits[0].pose.position);
			_isPumpPlaced = true;
			ObjectPlaced.Invoke();

			_planeManager.enabled = false;
			_pointCloudManager.enabled = false;
			_planeManager.SetTrackablesActive(false);
			_pointCloudManager.SetTrackablesActive(false);
		}
	}
}
