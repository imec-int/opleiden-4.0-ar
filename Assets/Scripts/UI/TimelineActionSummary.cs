using UnityEngine;
using TMPro;
using TimeLineValidation;
using Data;
using Core;
using System.Linq;

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

		private void UpdateText(ValidationStageReport report)
		{
			// Ensure visibility; It's turned on/off in reset state
			this.gameObject.SetActive(true);

			int placed = report.PerformedActionsValidationResult.Count - report.PerformedActionsValidationResult.Count(item => item.Result != Result.Correct);

			string hex = ColorUtility.ToHtmlStringRGB(_colorScheme.ValidationColorDictionary[Result.Correct].normalColor);
			_actionText.text = $"Acties: <color=#{hex}>{placed}</color>/{report.RequiredActions}";
		}
	}
}
