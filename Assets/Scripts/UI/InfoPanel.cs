using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Data;

namespace UI
{
	public class InfoPanel : MonoBehaviour
	{
		public event Action OnOpen;
		public event Action OnClose;

		[SerializeField]
		private TextMeshProUGUI _HeaderLabel;

		[SerializeField]
		private TextMeshProUGUI _BodyLabel;

		[SerializeField]
		private Button _CloseButton;

		public void Awake()
		{
			_CloseButton.onClick.AddListener(Close);
		}

		public void Show(HighlightInfo info)
		{
			Show(info.Header, info.Body);
		}

		public void Show(string header, string body)
		{
			OnOpen?.Invoke();
			_HeaderLabel.text = header;
			_BodyLabel.text = body;

			this.gameObject.SetActive(true);
		}

		public void Close()
		{
			OnClose?.Invoke();
			this.gameObject.SetActive(false);
		}
	}
}
