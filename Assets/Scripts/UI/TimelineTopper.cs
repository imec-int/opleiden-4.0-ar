using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using TimeLineValidation;
using Data;

namespace UI
{
	public class TimelineTopper : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI _actionText;

		[SerializeField]
		private ActionController _controller;

		[SerializeField]
		private ColorScheme _colorScheme;

		void Awake()
		{
			_controller.ValidationCompleted += UpdateTopper;
		}

		private void UpdateTopper(ValidationInfo info)
		{
			// Ensure visibility; It's turned on/off in reset state
			this.gameObject.SetActive(true);

			int placed = info.ValidationResultList.Count-info.AmountOfErrors;
			uint needed = info.UsedRuleSet.TotalStepCount;
			var result = placed == needed ? ValidationResult.Correct: ValidationResult.Incorrect;

			string hex = _colorScheme.ValidationColorDictionary[result].normalColor.ToHexCodeString();
			_actionText.text = $"Acties: <color={hex}>{placed}</color>/{needed}";
		}
	}
}