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

		private PumpAnchor _pumpAnchor;

		[SerializeField]
		private TMP_InputField _inputField;

		[SerializeField]
		private InfoPanel _infoPanel;

		[SerializeField]
		private ErrorPopup _errorPopup;

		private TouchScreenKeyboard _touchScreenKeyboard;

		[SerializeField]
		private string _errorMessage = "Je staat niet aan de correcte pomp, voor meer info zie '?'";

		protected void Awake()
		{
			_pumpAnchor = GameObject.FindGameObjectWithTag("Installation").GetComponentInChildren<PumpAnchor>();
			transform.position = _pumpAnchor.transform.position;
			_inputField.characterLimit = _pumpAnchor.PumpID.Length;
			_touchScreenKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.ASCIICapable, false, false, false, false, "Pump ID", 20);
		}

		private void OnInfoPanelClosed()
		{
			this.gameObject.SetActive(true);
			_infoPanel.OnClose -= OnInfoPanelClosed;
		}

		protected void OnEnable()
		{
			// Focuses inputfield on activation inside coroutine because Unity still has to activate the inputfield.
			StartCoroutine(FocusInputField());
		}

		private IEnumerator FocusInputField()
		{
			// We should only read the screen buffer after rendering is complete
			yield return new WaitForEndOfFrame();
			_inputField.interactable = true;
			_inputField.Select();
		}

		public void OnValueChanged()
		{
			if(_inputField.text.Length == _pumpAnchor.PumpID.Length)
			{
				if (string.Equals(_inputField.text, _pumpAnchor.PumpID, System.StringComparison.OrdinalIgnoreCase))
				{
					_inputField.interactable = false;
					_inputField.GetComponent<Image>().CrossFadeColor(Color.green, 0.5f, false, false);
					_animator.SetTrigger("PumpTagged");
					_touchScreenKeyboard.active = false;
				}
				else
				{
					ShowErrorMessage();
				}
			}
		}

		public void ShowErrorMessage()
		{
			Reset();
			_touchScreenKeyboard.active = false;
			this.gameObject.SetActive(false);
			_errorPopup.OnClosed += OnErrorPopupClosed;
			_errorPopup.Show(_errorMessage.ToUnicodeForTMPro());
		}

		private void OnErrorPopupClosed()
		{
			this.gameObject.SetActive(true);
			_errorPopup.OnClosed -= OnErrorPopupClosed;
		}

		public void ShowAdditionalInfo()
		{
			this.gameObject.SetActive(false);
			_infoPanel.Show(_serialInfo);
			_infoPanel.OnClose += OnInfoPanelClosed;
		}

		public void Reset()
		{
			_inputField.text = string.Empty;
		}
	}
}
