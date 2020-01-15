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
		private MyDebug myDebug;

		private bool isPumpPlaced;
		protected void Awake()
		{
			_sessionOrigin = GetComponent<ARSessionOrigin>();
			_raycastManager = GetComponent<ARRaycastManager>();

			myDebug = GameObject.Find("DebugField").GetComponent<MyDebug>();
		}


		protected void Update()
		{
			if (Input.touchCount == 0)
				return;

			List<ARRaycastHit> hits = new List<ARRaycastHit>();
  
			if (Input.touchCount == 1 && _raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
			{
                if(!isPumpPlaced)
				PlacePump(hits);
			}
		}

		void PlacePump(List<ARRaycastHit> hits) {
			ObjectPlaced.Invoke();
			_installation = Instantiate(_installationPrefab).transform;
			_sessionOrigin.MakeContentAppearAt(_installation, hits[0].pose.position);
			isPumpPlaced = true;

			myDebug.Debug("PlacePump func");
			//transform.localScale = new Vector3(1 / _installationScale, 1 / _installationScale, 1 / _installationScale);
		}

	}
}
