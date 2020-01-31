using System;
using System.Collections;
using System.Collections.Generic;
using AR;
using RotaryHeart.Lib.SerializableDictionary;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
	public class TopInfoRibbon : MonoBehaviour
	{
		protected static TopInfoRibbon _instance = null;
		public static TopInfoRibbon Instance => _instance;

		private TextMeshProUGUI _textLabel;

		protected void Awake()
		{
			_textLabel = this.GetComponentInChildren<TextMeshProUGUI>();
			Assert.IsNotNull(_textLabel);
			_instance = this;
			this.gameObject.SetActive(false);
		}

		public void SetLabelText(string text)
		{
			_textLabel.text = text;
		}
	}
}