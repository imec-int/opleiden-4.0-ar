using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace AR
{
	public enum TrackingType
	{
		None = 0,
		Object,
		Plane
	}

	[RequireComponent(typeof(ARPlaneManager), typeof(ARPointCloudManager), typeof(ObjectPlacement))]
	// [RequireComponent(typeof(ARTrackedObjectManager))]
	public class TrackingController : MonoBehaviour
	{
		private ARPlaneManager _arPlaneManager;
		private ARPointCloudManager _arPointCloudManager;
		private ObjectPlacement _objectPlacement;
		private ARTrackedObjectManager _arTrackedObjectManager;

		[SerializeField]
		private Vector3 _objectTrackingOffset;

		[SerializeField]
		private Vector3 _objectTrackingOffsetRotation;

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private ARPlacedObject _installationPrefab;

		[SerializeField]
		private ARSession _arSession;

		[SerializeField]
		private XRReferenceObjectLibrary _referenceLibrary;

		public TrackingType TrackingType { get; set; }

		private ARPlacedObject _installation;

		protected void Awake()
		{
			Assert.IsNotNull(_animator, "Animator is not filled in.");
			Assert.IsNotNull(_arSession, "AR Session is not filled in");

			_arPlaneManager = GetComponent<ARPlaneManager>();
			_arPointCloudManager = GetComponent<ARPointCloudManager>();
			_objectPlacement = GetComponent<ObjectPlacement>();
			// _arTrackedObjectManager = GetComponent<ARTrackedObjectManager>();
		}

		protected void OnEnable()
		{
			switch (Application.platform)
			{
				case RuntimePlatform.Android:
					EnablePlaneTracking();
					break;
				case RuntimePlatform.IPhonePlayer:
					switch (TrackingType)
					{
						case TrackingType.Plane:
							EnablePlaneTracking();
							break;
						default:
							EnableObjectTracking();
							break;
					}
					break;
				default:
					UnsupportedPlatform();
					break;
			}
		}

		protected void OnDisable()
		{
			Reset();

			// _arTrackedObjectManager.enabled = false;
			if (_arTrackedObjectManager)
			{
				_arTrackedObjectManager.trackedObjectsChanged -= OnTrackedObjectsChanged;
				Destroy(_arTrackedObjectManager);
			}
			_objectPlacement.enabled = false;
			_arPlaneManager.enabled = false;
			_arPointCloudManager.enabled = false;
		}

		private void OnTrackedObjectsChanged(ARTrackedObjectsChangedEventArgs changedTrackedObjects)
		{
			if (changedTrackedObjects.added.Count > 0)
			{
				for (int i = 0; i < changedTrackedObjects.added.Count; i++)
				{
					changedTrackedObjects.added[i].GetComponent<ARPlacedObject>()?.ShowModel(false);
					
					changedTrackedObjects.added[i].transform.GetChild(0).localPosition = _objectTrackingOffset;
					changedTrackedObjects.added[i].transform.GetChild(0).localRotation = Quaternion.Euler(_objectTrackingOffsetRotation);
				}
				TrackingCompleted();
			}
		}

		private void EnablePlaneTracking()
		{
			_objectPlacement.enabled = true;
			_arPlaneManager.enabled = true;
			_arPointCloudManager.enabled = true;
			_arPointCloudManager.SetTrackablesActive(true);
		}

		private void EnableObjectTracking()
		{
			_objectPlacement.enabled = false;
			_arPlaneManager.enabled = false;
			_arPointCloudManager.enabled = false;

			// _arTrackedObjectManager.enabled = true;
			_arTrackedObjectManager = gameObject.AddComponent<ARTrackedObjectManager>();
			_arTrackedObjectManager.referenceLibrary = _referenceLibrary;
			_arTrackedObjectManager.trackedObjectPrefab = _installationPrefab.gameObject;
			_arTrackedObjectManager.trackedObjectsChanged += OnTrackedObjectsChanged;
			_arTrackedObjectManager.enabled = true;
		}

		private void UnsupportedPlatform()
		{
			_arPlaneManager.enabled = false;
			_arPointCloudManager.enabled = false;
			_objectPlacement.enabled = false;

			_installation = Instantiate(_installationPrefab);
			_installation.name = _installationPrefab.name;
			TrackingCompleted();
		}

		public void Reset()
		{
			_arPlaneManager.SetTrackablesActive(false);
			_arPointCloudManager.SetTrackablesActive(false);
			if (_arTrackedObjectManager)
				_arTrackedObjectManager.SetTrackablesActive(false);
			_arSession.Reset();
			if (_objectPlacement.Reset()) return;
			if (!_installation) return;
			Destroy(_installation.gameObject);
		}

		public void TrackingCompleted()
		{
			_animator.SetTrigger("CalibrationComplete");
		}
	}
}
