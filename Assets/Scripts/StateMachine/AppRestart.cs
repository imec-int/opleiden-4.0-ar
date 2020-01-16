﻿using System.Collections;
using System.Collections.Generic;
using Core;
using UI;
using UnityEngine;

namespace StateMachine {
  public class AppRestart : StateMachineBehaviour {
    private bool _shouldRun = true;

    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      _shouldRun = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      // Run (ONCE) after all the states that do something on enter
      if (!_shouldRun)
        return;

      animator.GetComponent<ActionController> ().Reset ();
      animator.GetComponent<StateVariableHolder> ().Widgets[Widget.PumpTag].GetComponent<PumpTagField> ().Reset ();
      animator.SetTrigger ("ResetSuccessful");
      // HACK for now, until the final logic is here
      // animator.SetTrigger("CalibrationComplete");

      _shouldRun = false;
    }
  }
}