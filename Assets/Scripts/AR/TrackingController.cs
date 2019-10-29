using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace AR
{
	[RequireComponent(typeof(ARPlaneManager), typeof(ARPointCloudManager))]
	public class TrackingController : MonoBehaviour
	{
		private ARPlaneManager _arPlaneManager;
		private ARPointCloudManager _arPointCloudManager;
		private ObjectPlacement _objectPlacement;
		private ARTrackedObjectManager _arTrackedObjectManager;

		protected void Awake()
		{
			_arPlaneManager = GetComponent<ARPlaneManager>();
			_arPointCloudManager = GetComponent<ARPointCloudManager>();
			_objectPlacement = GetComponent<ObjectPlacement>();
			_arTrackedObjectManager = GetComponent<ARTrackedObjectManager>();

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
		}
	}
}
