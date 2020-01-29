using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class ThreeDotsWidget : MonoBehaviour
	{
		[SerializeField]
		private Animator _stateMachine;

		[SerializeField]
		private GameObject _submenu;

		[SerializeField]
		private ColorScheme _colorScheme;

		[SerializeField]
		private InfoPanel _infoPanel;

		private Button _button;
		private bool _expanded;

		#region Monobehaviour
		protected void Awake()
		{
			_button = this.GetComponent<Button>();
			_button.onClick.AddListener(ToggleSubmenu);
			CloseSubmenu();

			foreach (Button btn in GetComponentsInChildren(typeof(Button), true))
			{
				btn.colors = _colorScheme.UIColorsDictionary[ColorStyleables.Button];
			}

			_infoPanel.OnClose += () => this.gameObject.SetActive(true);
			_infoPanel.OnOpen += () => this.gameObject.SetActive(false);
		}
		#endregion

		public void ToggleSubmenu()
		{
			_submenu.SetActive(!_submenu.activeSelf);
			_expanded = !_expanded;
		}

		public void ExpandSubmenu()
		{
			_expanded = true;
			_submenu.SetActive(true);
		}

		public void CloseSubmenu()
		{
			_expanded = false;
			_submenu.SetActive(false);
		}

		public void ResetApplication()
		{
			_stateMachine.SetTrigger("ResetRequested");
			CloseSubmenu();
		}
	}
}
