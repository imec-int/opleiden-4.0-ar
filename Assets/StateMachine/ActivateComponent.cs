using UnityEngine;

namespace StateMachine
{
	public class ActivateComponent : StateMachineBehaviour
	{
		[SerializeField]
		private Component _component;

		[SerializeField]
		private bool _activateOnEnter = true, _deactivateOnExit = true;

		private MonoBehaviour _associatedComponent;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_associatedComponent = animator.GetComponent<StateVariableHolder>().Components[_component];

			Debug.Assert(_associatedComponent, $"{_component} widget was not found");

			if (_activateOnEnter) _associatedComponent.enabled = true;
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_deactivateOnExit) _associatedComponent.enabled = false;
		}
	}
}
