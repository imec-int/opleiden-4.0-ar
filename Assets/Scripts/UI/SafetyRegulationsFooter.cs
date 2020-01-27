using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class SafetyRegulationsFooter : MonoBehaviour
	{
		// Checkbox
		[SerializeField]
		private Toggle _complianceCheckBox;
		// Button
		[SerializeField]
		private Button _continueButton;
		// Info panel
		[SerializeField]
		private InfoPanel _currentInfoPanel;

		[SerializeField]
		// State Machine
		private Animator _stateMachine;

		protected void Awake()
		{
			// Subscribe to events
			_complianceCheckBox.onValueChanged.AddListener(OnCheckBoxChanged);
			_continueButton.onClick.AddListener(OnButtonClicked);
		}

		// Toggle button interactable
		protected void OnCheckBoxChanged(bool value)
		{
			_continueButton.interactable = value;
		}

		// Trigger animator change state
		protected void OnButtonClicked()
		{
			_stateMachine.SetTrigger("EndSafetyCheck");
			_currentInfoPanel.Close();
		}
	}
}