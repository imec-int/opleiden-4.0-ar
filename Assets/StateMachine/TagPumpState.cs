using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagPumpState : StateMachineBehaviour
{
	private GameObject _AssociatedUIElement = null;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!_AssociatedUIElement)
		{
			_AssociatedUIElement = animator.GetComponent<StateVariableHolder>().PumpTagField;
		}
		_AssociatedUIElement.SetActive(true);

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_AssociatedUIElement.SetActive(false);
	}
}
