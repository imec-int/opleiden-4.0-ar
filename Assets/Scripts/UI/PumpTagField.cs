using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(LookAtCamera))]
	public class PumpTagField : MonoBehaviour
	{
		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private PumpAnchor _pumpAnchor;

		[SerializeField]
		private TMP_InputField _inputField;

		private void Awake()
		{
			_inputField.characterLimit = _pumpAnchor.PumpID.Length;
		}

		private void OnEnable()
		{
			_inputField.Select();
		}

		public void OnValueChanged()
		{
			if (_inputField.text == _pumpAnchor.PumpID)
			{
				_inputField.interactable = false;
				_inputField.GetComponent<Image>().color = Color.green;
				_animator.SetTrigger("PumpTaggedComplete");

			}
		}
	}
}
