using UnityEngine;

namespace Core
{
	public class Report : MonoBehaviour
	{
		[SerializeField]
		private Animator _stateMachine;

		private ActionController _actionController;

		private void Awake()
		{
			_actionController = GetComponent<ActionController>();
			_actionController.ValidationCompleted += CheckReport;
		}

		private void CheckReport(ValidationStageReport stageReport)
		{
			if (!stageReport.Succeeded)
			{
				_stateMachine.SetTrigger("ValidationSuccessfull");
			}
		}
	}
}
