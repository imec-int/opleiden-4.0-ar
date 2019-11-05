using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.ARFoundation;

namespace AR
{
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
		private GameObject _installationPrefab;

		protected void Awake()
		{
			Assert.IsNotNull(_animator, "Animator is not filled in.");

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
					EnableObjectTracking();
					break;
				default:
					UnsupportedPlatform();
					break;
			}
		}

		protected void OnDisable()
		{
			_arTrackedObjectManager.trackedObjectsChanged -= OnTrackedObjectsChanged;

			_arTrackedObjectManager.enabled = false;
			_objectPlacement.enabled = false;
			_arPlaneManager.enabled = false;
			_arPointCloudManager.enabled = false;
		}

		private void OnTrackedObjectsChanged(ARTrackedObjectsChangedEventArgs changedTrackedObjects)
		{
			if (changedTrackedObjects.added.Count > 0)
			{
				TrackingCompleted();
			}
		}

		private void EnablePlaneTracking()
		{
			_arTrackedObjectManager.enabled = false;

			_objectPlacement.enabled = true;
			_arPlaneManager.enabled = true;
			_arPointCloudManager.enabled = true;
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

			Instantiate(_installationPrefab).name = _installationPrefab.name;
			TrackingCompleted();
		}

		public void TrackingCompleted()
		{
			_animator.SetTrigger("CalibrationComplete");
		}
	}
}
