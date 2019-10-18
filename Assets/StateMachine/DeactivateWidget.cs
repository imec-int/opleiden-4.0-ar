using UnityEngine;

namespace StateMachine
{
	public class DeactivateWidget : StateMachineBehaviour
	{
		[SerializeField]
		private Widget _widget;

		[SerializeField]
		private bool _deactivateOnEnter = true, _activateOnExit = true;

		private GameObject _AssociatedWidget;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_AssociatedWidget = animator.GetComponent<StateVariableHolder>()?.Widgets[_widget];

			Debug.Assert(_AssociatedWidget, $"{_widget} widget was not found");

			if (_deactivateOnEnter) _AssociatedWidget?.SetActive(false);
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_activateOnExit) _AssociatedWidget?.SetActive(true);
		}
	}
}
