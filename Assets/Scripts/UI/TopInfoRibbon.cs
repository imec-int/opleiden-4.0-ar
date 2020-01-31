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
		[Serializable] public class TrackingStringDictionary : SerializableDictionaryBase<TrackingType, string> { }


		[SerializeField]
		private Animator _stateMachine;

		[SerializeField]
		private TrackingController _trackingController;

		[SerializeField]
		private TrackingStringDictionary _infoLabels;

		private const string _calibrationStateName = "ARCalibration";
		private TextMeshProUGUI _textLabel;

		protected void Awake()
		{
			_textLabel = this.GetComponentInChildren<TextMeshProUGUI>();
			Assert.IsNotNull(_textLabel);
			Assert.IsTrue(_infoLabels.Count > 0);
		}

		protected void OnEnable()
		{
			var info = _stateMachine.GetCurrentAnimatorStateInfo(0);
			if (info.IsName(_calibrationStateName))
			{
				SetLabelText(_infoLabels[_trackingController.TrackingType]);
			}
		}

		public void SetLabelText(string text)
		{
			_textLabel.text = text;
		}
	}
}