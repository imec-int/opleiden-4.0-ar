using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenState : StateMachineBehaviour
{
    [SerializeField]
    private float _FadeTimer = 2.0f;
    private float _ScreenTime = -100;
    private GameObject _AssociatedUIElement = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_AssociatedUIElement)
        {
            _AssociatedUIElement = animator.GetComponent<StateVariableHolder>().LoadingScreen;
        }
       _AssociatedUIElement.SetActive(true);
       _ScreenTime = Time.time;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(Time.time >= _ScreenTime + _FadeTimer)
        {
            animator.SetTrigger("StartCalibration");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       _AssociatedUIElement.SetActive(false);
    }
}
