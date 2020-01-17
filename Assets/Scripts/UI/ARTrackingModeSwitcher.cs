using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AR;
using Data;
using System;

namespace UI
{
	public class ARTrackingModeSwitcher : MonoBehaviour
	{
		[SerializeField]
		private Button _objectTrackingButton;

		[SerializeField]
		private Button _planeTrackingButton;

		[SerializeField]
		private Button _infoButton;

		[SerializeField]
		private InfoPanel _infoPanel;

		[SerializeField]
		private Animator _stateMachineAnimator;

		[SerializeField]
		private TrackingController _trackingController;

		[SerializeField]
		private HighlightInfo _highlightInfo;

		protected readonly static string _stateTrigger = "StartCalibration";

		protected void Awake()
		{
			_objectTrackingButton.onClick.AddListener(OnObjTrackingClicked);
			_planeTrackingButton.onClick.AddListener(OnPlaneTrackingClicked);
			_infoButton.onClick.AddListener(OnInfoRequested);
			_infoPanel.OnClose += OnInfoPanelClosed;
		}

		public void OnInfoPanelClosed()
		{
			this.gameObject.SetActive(false);
		}

		protected void OnInfoRequested()
		{
			_infoPanel.Show(_highlightInfo);
			this.gameObject.SetActive(false);
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
