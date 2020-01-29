using UnityEngine;
using Data;
using UI;
using System.Collections.Generic;

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

		[SerializeField]
		private List<InfoPanelFooter> _additionalPrefabs;

		private InfoPanel _infoPanel;
		private Animator _animator;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_infoPanel = animator.GetComponent<StateVariableHolder>().Components[_component] as InfoPanel;

			Debug.Assert(_infoPanel, $"{_component} component was not found");

			_infoPanel.Show(_info, _showCloseBtn, _tapToClose);
			foreach (var prefab in _additionalPrefabs)
			{
				InfoPanelFooter footer = _infoPanel.PushExtraInfoPrefab(prefab);
				footer.StateMachine = animator;
			}
			if (_showCloseBtn || _tapToClose)
			{
				_animator = animator;
				_infoPanel.OnClose += OnInfoPanelClosed;
			}
		}

		private void OnInfoPanelClosed()
		{
			_infoPanel.OnClose -= OnInfoPanelClosed;
			_animator.SetTrigger("InfoShownComplete");
		}
	}
}
