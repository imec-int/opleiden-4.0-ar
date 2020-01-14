using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AR;

namespace UI
{
	public class ARTrackingModeSwitcher : MonoBehaviour
	{
		[SerializeField]
		private Button _objectTrackingButton;

		[SerializeField]
		private Button _planeTrackingButton;

		[SerializeField]
		private Animator _stateMachineAnimator;

		[SerializeField]
		private TrackingController _trackingController;

		protected readonly static string _stateTrigger = "StartCalibration";

		protected void Awake()
		{
			_objectTrackingButton.onClick.AddListener(OnObjTrackingClicked);
			_planeTrackingButton.onClick.AddListener(OnPlaneTrackingClicked);
		}

		protected void OnObjTrackingClicked()
		{
			_trackingController.TrackingType = TrackingType.Object;
			TriggerTransition();
		}

		protected void OnPlaneTrackingClicked()
		{
			_trackingController.TrackingType = TrackingType.Plane;
			TriggerTransition();
		}

		protected void TriggerTransition()
		{
			_stateMachineAnimator.SetTrigger(_stateTrigger);
		}
	}
}
