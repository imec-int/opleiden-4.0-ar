using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using TimeLineValidation;
using Data;

namespace UI
{
	public class TimelineActionSummary : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI _actionText;

		[SerializeField]
		private ActionController _controller;

		[SerializeField]
		private ColorScheme _colorScheme;

		void Awake()
		{
			_controller.ValidationCompleted += UpdateText;
		}

		private void UpdateText(ValidationInfo info)
		{
			// Ensure visibility; It's turned on/off in reset state
			this.gameObject.SetActive(true);

			int placed = info.ValidationResultList.Count-info.AmountOfErrors;
			uint needed = info.UsedRuleSet.TotalStepCount;
			var result = placed == needed ? ValidationResult.Correct: ValidationResult.Incorrect;

			string hex = ColorUtility.ToHtmlStringRGB(_colorScheme.ValidationColorDictionary[result].normalColor);
			_actionText.text = $"Acties: <color=#{hex}>{placed}</color>/{needed}";
		}
	}
}