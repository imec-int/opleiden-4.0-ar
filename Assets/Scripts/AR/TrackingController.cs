using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.ARFoundation;

namespace AR
{
	public enum TrackingType
	{
		None = 0,
		Object,
		Plane
	}

	[RequireComponent(typeof(ARPlaneManager), typeof(ARPointCloudManager), typeof(ObjectPlacement))]
	[RequireComponent(typeof(ARTrackedObjectManager))]
	public class TrackingController : MonoBehaviour
	{
		private ARPlaneManager _arPlaneManager;
		private ARPointCloudManager _arPointCloudManager;
		private ObjectPlacement _objectPlacement;
		private ARTrackedObjectManager _arTrackedObjectManager;

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private ARPlacedObject _installationPrefab;

		[SerializeField]
		private ARSession _arSession;

		public TrackingType TrackingType { get; set; }

		private ARPlacedObject _installation;

		protected void Awake()
		{
			Assert.IsNotNull(_animator, "Animator is not filled in.");
			Assert.IsNotNull(_arSession, "AR Session is not filled in");

			_arPlaneManager = GetComponent<ARPlaneManager>();
			_arPointCloudManager = GetComponent<ARPointCloudManager>();
			_objectPlacement = GetComponent<ObjectPlacement>();
			_arTrackedObjectManager = GetComponent<ARTrackedObjectManager>();
		}

		protected void OnEnable()
		{
			_arTrackedObjectManager.trackedObjectsChanged += OnTrackedObjectsChanged;

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
			_arTrackedObjectManager.trackedObjectsChanged -= OnTrackedObjectsChanged;

			Reset();

			_arTrackedObjectManager.enabled = false;
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
				}
				TrackingCompleted();
			}
		}

		private void EnablePlaneTracking()
		{
			_arTrackedObjectManager.enabled = false;

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

			_arTrackedObjectManager.enabled = true;
		}

		private void UnsupportedPlatform()
		{
			_arPlaneManager.enabled = false;
			_arPointCloudManager.enabled = false;
			_objectPlacement.enabled = false;
			_arTrackedObjectManager.enabled = false;

			_installation = Instantiate(_installationPrefab);
			_installation.name = _installationPrefab.name;
			TrackingCompleted();
		}

		public void Reset()
		{
			_arPlaneManager.SetTrackablesActive(false);
			_arPointCloudManager.SetTrackablesActive(false);
			_arTrackedObjectManager.SetTrackablesActive(false);
			_arSession.Reset();
			if(_objectPlacement.Reset()) return;
			if(!_installation) return;
			Destroy(_installation.gameObject);
		}

		public void TrackingCompleted()
		{
			_animator.SetTrigger("CalibrationComplete");
		}
	}
}
