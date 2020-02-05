using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class ReportFooter : InfoPanelFooter
	{
		[SerializeField]
		private Button _closeButton;

		protected void Awake()
		{
			// Subscribe to events
			_closeButton.onClick.AddListener(OnButtonClicked);
		}

		private void OnButtonClicked()
		{
			StateMachine.SetTrigger("ResetRequested");
			_closeButton.onClick.RemoveListener(OnButtonClicked);
		}
	}
}
