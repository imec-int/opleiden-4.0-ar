using UnityEngine;
using System.Collections.Generic;
using Data;

namespace TimeLineValidation
{
	[CreateAssetMenu(fileName = "OrderedSequence", menuName = "opleiden-4.0-ar/OrderedSequence", order = 1)]
	public class OrderedActionSequence : ActionSequence
	{
		public override void Validate(List<ActionData> timelineActions, out ValidationInfo outValidationInfo)
		{
			base.Validate(timelineActions, out outValidationInfo);

			// // Reset
			// _actionPhaseIndex = 0;
			// _subStepIndex = 0;

			// bool succeeded = true;
			// // Go through all time line actions
			// foreach (ActionData action in allTimelineActions)
			// {
			// 	ValidationResult result = ValidateAction(action);
			// 	if (result != ValidationResult.Correct)
			// 		succeeded = false;
			// 	// Add result to list
			// 	outValidationInfo.ValidationResultList.Add(result);
			// }
			// outValidationInfo.Succeeded = succeeded && TotalSteps == allTimelineActions.Count;
		}

		// 		private ValidationResult RecursivelyPhaseCheck(ActionData action)
		// 		{
		// 			Step currentActionPhase = _actionPhaseList.ElementAtOrDefault(_actionPhaseIndex);

		// 			// If we go past the last phase, reset index to previous and exit;
		// 			if (currentActionPhase == null)
		// 			{
		// 				--_actionPhaseIndex;
		// 				_subStepIndex = 0;
		// 				// default: wrong position // We already checked whether it's unnecessary
		// 				return ValidationResult.IncorrectPosition;
		// 			}
		// 			if (currentActionPhase.SubstepUIDList.Contains(action.UID))
		// 			{
		// 				if (currentActionPhase.SubStepOrderMatters)
		// 				{
		// 					ActionData currentStep = currentActionPhase.SubSteps.ElementAtOrDefault(_subStepIndex);
		// 					Debug.Assert(currentStep != null);
		// 					// Increase substep
		// 					++_subStepIndex;
		// 					// Validate
		// 					return currentStep.UID == action.UID ? ValidationResult.Correct : ValidationResult.IncorrectPosition;
		// 				}
		// 				else
		// 				{
		// 					return ValidationResult.Correct;
		// 				}
		// 			}
		// 			else // Check next phase
		// 			{
		// 				++_actionPhaseIndex; _subStepIndex = 0;
		// 				return RecursivelyPhaseCheck(action);
		// 			}
		// 		}

		// 		private ValidationResult ValidateAction(ActionData action)
		// 		{
		// 			// Check unnecessary
		// 			if (!_uidUsageInRulesetDict.ContainsKey(action.UID))
		// 			{
		// 				return ValidationResult.Incorrect;
		// 			}
		// 			// Check incorrect placement
		// 			ValidationResult result = RecursivelyPhaseCheck(action);

		// 			// Too many of the same UID check
		// 			int count = IncreaseUsageForUID(_uidUsageInTimelineDict, action.UID);
		// 			if (count > _uidUsageInRulesetDict[action.UID])
		// 			{
		// 				return ValidationResult.Incorrect;
		// 			}
		// 			return result;
		// 		}
		// #endregion

		// 		#region Helpers
		// 		private int IncreaseUsageForUID(Dictionary<int, int> dict, int UID)
		// 		{
		// 			if (dict.TryGetValue(UID, out int usage))
		// 			{
		// 				dict[UID] = ++usage;
		// 			}
		// 			else
		// 				dict[UID] = 1;

		// 			return dict[UID];
		// 		}
		// 		#endregion
	}
}
