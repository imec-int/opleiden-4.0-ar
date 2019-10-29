using UnityEngine;
using TMPro;
using TimeLineValidation;
using Data;
using Core;

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

		private void Awake()
		{
			_controller.ValidationCompleted += UpdateText;
		}

		private void UpdateText(ValidationInfo info)
		{
			// Ensure visibility; It's turned on/off in reset state
			this.gameObject.SetActive(true);

			int placed = info.ValidationResultList.Count - info.AmountOfErrors;
			uint needed = info.UsedRuleSet.TotalStepCount;
			ValidationResult result = placed == needed ? ValidationResult.Correct : ValidationResult.Incorrect;

			string hex = ColorUtility.ToHtmlStringRGB(_colorScheme.ValidationColorDictionary[result].normalColor);
			_actionText.text = $"Acties: <color=#{hex}>{placed}</color>/{needed}";
		}
	}
}
