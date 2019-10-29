using System.Collections;
using Data;
using Develop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
	[RequireComponent(typeof(LookAtCamera))]
	public class PumpTagField : MonoBehaviour
	{
		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private HighlightInfo _serialInfo;

		[SerializeField]
		private PumpAnchor _pumpAnchor;

		[SerializeField]
		private TMP_InputField _inputField;

		[SerializeField]
		private InfoPanel _infoPanel;

		private TouchScreenKeyboard _touchScreenKeyboard;

		private void Awake()
		{
			_inputField.characterLimit = _pumpAnchor.PumpID.Length;
			_touchScreenKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.ASCIICapable, false, false, false, false, "Pump ID", 20);
		}

		private void OnInfoPanelClosed()
		{
			this.gameObject.SetActive(true);
			_infoPanel.OnClose -= OnInfoPanelClosed;
		}

		private void OnEnable()
		{
			// Focuses inputfield on activation inside coroutine because Unity still has to activate the inputfield.
			StartCoroutine(FocusInputField());
		}

		private IEnumerator FocusInputField()
		{
			// We should only read the screen buffer after rendering is complete
			yield return new WaitForEndOfFrame();

			_inputField.Select();
		}

		public void OnValueChanged()
		{
			if (_inputField.text == _pumpAnchor.PumpID)
			{
				_inputField.interactable = false;
				_inputField.GetComponent<Image>().CrossFadeColor(Color.green, 0.5f, false, false);
				_animator.SetTrigger("PumpTagged");
				_touchScreenKeyboard.active = false;
			}
		}

		public void ShowAdditionalInfo()
		{
			this.gameObject.SetActive(false);
			_infoPanel.Show(_serialInfo);
			_infoPanel.OnClose += OnInfoPanelClosed;
		}
	}
}
