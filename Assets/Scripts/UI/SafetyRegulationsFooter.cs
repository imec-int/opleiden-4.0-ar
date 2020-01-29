using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class SafetyRegulationsFooter : InfoPanelFooter
	{
		// Checkbox
		[SerializeField]
		private Toggle _complianceCheckBox;
		// Button
		[SerializeField]
		private Button _continueButton;

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
			StateMachine.SetTrigger("SafetyCheckComplete");
			StateMachine.SetBool("FirstRun",false);
			ParentPanel.Close();
		}
	}
}