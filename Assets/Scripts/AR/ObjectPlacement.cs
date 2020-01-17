using System;
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

		private bool _isPumpPlaced;
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
  
			if (Input.touchCount == 1 && _raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
			{
                if(!_isPumpPlaced)
				PlacePump(hits);
			}
		}

		void PlacePump(List<ARRaycastHit> hits) {
			_installation = Instantiate(_installationPrefab).transform;
			_sessionOrigin.MakeContentAppearAt(_installation, hits[0].pose.position);
			_isPumpPlaced = true;
			ObjectPlaced.Invoke();
		}

	}
}
