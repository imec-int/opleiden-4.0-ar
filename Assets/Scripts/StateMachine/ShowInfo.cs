using UnityEngine;
using Data;
using UI;

namespace StateMachine
{
	public class ShowInfo : StateMachineBehaviour
	{
		[SerializeField]
		private Component _component;

		[SerializeField]
		private HighlightInfo _info;

		[SerializeField]
		private bool _showCloseBtn = true;

		[SerializeField]
		private bool _tapToClose;

		private InfoPanel _infoPanel;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_infoPanel = animator.GetComponent<StateVariableHolder>().Components[_component] as InfoPanel;

			Debug.Assert(_infoPanel, $"{_component} component was not found");

			_infoPanel.Show(_info, _showCloseBtn, _tapToClose);
			_infoPanel.OnClose += () => animator.SetTrigger("InfoShownComplete");
		}
	}
}
