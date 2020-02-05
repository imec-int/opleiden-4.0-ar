using System;
using System.Collections;
using System.Collections.Generic;
using AR;
using RotaryHeart.Lib.SerializableDictionary;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI
{
	public class TopInfoRibbon : MonoBehaviour
	{
		protected static TopInfoRibbon _instance = null;
		public static TopInfoRibbon Instance => _instance;

		[SerializeField]
		private InfoPanel _infoPanel;

		private TextMeshProUGUI _textLabel;

		protected void Awake()
		{
			_textLabel = this.GetComponentInChildren<TextMeshProUGUI>();
			Assert.IsNotNull(_textLabel);
			Assert.IsNotNull(_infoPanel);
			_instance = this;
			this.gameObject.SetActive(false);
			_infoPanel.OnOpen  += Hide;
			_infoPanel.OnClose += Show;
		}

		private void Show()
		{
			foreach(Graphic gra in this.GetComponentsInChildren<Graphic>())
			{
				gra.enabled = true;
			}
		}

		private void Hide()
		{
			foreach(Graphic gra in this.GetComponentsInChildren<Graphic>())
			{
				gra.enabled = false;
			}
		}

		public void SetLabelText(string text)
		{
			_textLabel.text = text;
		}
	}
}