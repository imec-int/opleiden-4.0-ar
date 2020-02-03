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
		protected readonly static string _objectTrackingInfo = "Mik de iPad® naar de pomp-installatie";
		protected readonly static string _planeTrackingInfo = "Klik in het gearceerde gebied om een pomp te plaatsen";

		protected void Awake()
		{
			_objectTrackingButton.onClick.AddListener(OnObjTrackingClicked);
			_planeTrackingButton.onClick.AddListener(OnPlaneTrackingClicked);
			_infoButton.onClick.AddListener(OnInfoRequested);
			_infoPanel.OnClose += OnInfoPanelClosed;
			_infoPanel.OnOpen += OnInfoPanelOpened;
		}

		private void OnInfoPanelOpened()
		{
			if (this.gameObject.activeSelf)
			{
				SetGraphicVisibility(false);
			}
		}

		private void SetGraphicVisibility(bool visible)
		{
			foreach (var graphic in GetComponentsInChildren<MaskableGraphic>(true))
			{
				graphic.enabled = visible;
			}
		}

		private void OnInfoPanelClosed()
		{
			if (this.gameObject.activeSelf)
			{
				SetGraphicVisibility(true);
			}
		}

		protected void OnInfoRequested()
		{
			_infoPanel.Show(_highlightInfo);
		}

		protected void OnObjTrackingClicked()
		{
			_trackingController.TrackingType = TrackingType.Object;
			TopInfoRibbon.Instance.SetLabelText(_objectTrackingInfo);
			TriggerTransition();
		}

		protected void OnPlaneTrackingClicked()
		{
			_trackingController.TrackingType = TrackingType.Plane;
			TopInfoRibbon.Instance.SetLabelText(_planeTrackingInfo);
			TriggerTransition();
		}

		protected void TriggerTransition()
		{
			_stateMachineAnimator.SetTrigger(_stateTrigger);
		}
	}
}
