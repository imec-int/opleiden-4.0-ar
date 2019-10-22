using UnityEngine;

namespace StateMachine
{
	public class ActivateWidget : StateMachineBehaviour
	{
		[SerializeField]
		private Widget _widget;

		[SerializeField]
		private bool _activateOnEnter = true, _deactivateOnExit = true;

		private GameObject _associatedWidget;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_associatedWidget = animator.GetComponent<StateVariableHolder>().Widgets[_widget];

			Debug.Assert(_associatedWidget, $"{_widget} widget was not found");

			if (_activateOnEnter) _associatedWidget.SetActive(true);
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_deactivateOnExit) _associatedWidget.SetActive(false);
		}
	}
}
