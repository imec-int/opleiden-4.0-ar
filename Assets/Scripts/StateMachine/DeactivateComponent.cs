using UnityEngine;

namespace StateMachine
{
	public class DeactivateComponent : StateMachineBehaviour
	{
		[SerializeField]
		private Component _component;

		[SerializeField]
		private bool _deactivateOnEnter = true, _activateOnExit = true;

		private MonoBehaviour _associatedComponent;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_associatedComponent = animator.GetComponent<StateVariableHolder>().Components[_component];

			Debug.Assert(_associatedComponent, $"{_component} widget was not found");

			if (_deactivateOnEnter) _associatedComponent.enabled = false;
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_activateOnExit) _associatedComponent.enabled = true;
		}
	}
}
