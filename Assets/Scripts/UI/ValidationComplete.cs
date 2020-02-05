using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class ValidationComplete : InfoPanelFooter
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
		}
	}
}
