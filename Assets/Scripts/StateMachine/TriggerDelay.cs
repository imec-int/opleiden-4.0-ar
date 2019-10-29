using UnityEngine;

namespace StateMachine
{
	public class TriggerDelay : StateMachineBehaviour
	{
		[SerializeField]
		private float _fadeTimer = 2.0f;

		[SerializeField]
		private string _triggerName;

		private float _screenTime = -100;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_screenTime = Time.time;
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (Time.time >= _screenTime + _fadeTimer)
			{
				animator.SetTrigger(_triggerName);
			}
		}
	}
}
