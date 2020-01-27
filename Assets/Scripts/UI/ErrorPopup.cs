using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
	public class ErrorPopup : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI _messageText;

		public event Action OnClosed;
		public event Action OnOpened;

		protected void Awake()
		{
			this.gameObject.SetActive(false);
		}

		public void Show(string text)
		{
			this.gameObject.SetActive(true);
			_messageText.SetText(text);
			_messageText.UpdateMeshPadding();
			OnOpened?.Invoke();
		}

		public void Close()
		{
			this.gameObject.SetActive(false);
			OnClosed?.Invoke();
		}

		protected void Update()
		{
			if(this.gameObject.activeSelf && Input.GetMouseButtonDown(0))
				Close();
		}
	}
}