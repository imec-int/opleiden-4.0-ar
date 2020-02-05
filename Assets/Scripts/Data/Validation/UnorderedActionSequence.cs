using UnityEngine;
using System.Collections.Generic;
using Data;
using System.Linq;

namespace TimeLineValidation
{
	[CreateAssetMenu(fileName = "UnorderedSequence", menuName = "opleiden-4.0-ar/UnorderedSequence", order = 1)]
	public class UnorderedActionSequence : ActionSequence
	{
		public override void Validate(List<ActionData> timelineActions, out ValidationInfo outValidationInfo)
		{
			base.Validate(timelineActions, out outValidationInfo);

			foreach (var item in timelineActions)
			{
				if (_actions.Contains(item))
				{
					bool allow = true;
					// Check if there are duplicate actions
					if (timelineActions.Count(a => a.GetHashCode() == item.GetHashCode()) > 1)
					{
						// There are duplicates so only allow the first one, the rest will be marked incorrect
						ActionData firstActionOfType = timelineActions.First(a => a.GetHashCode() == item.GetHashCode());
						allow = firstActionOfType == item;
						outValidationInfo.Succeeded = false;
					}
					outValidationInfo.PerformedActionsValidationResult.Add(new ValidationResult(allow || _allowDuplicateActions ? Result.Correct : Result.Incorrect, item));
				}
				else
				{
					outValidationInfo.PerformedActionsValidationResult.Add(new ValidationResult(Result.Incorrect, item));
				}
			}

			foreach (var item in _actions)
			{
				if (!timelineActions.Contains(item))
				{
					outValidationInfo.ForgottenActionsValidationResult.Add(new ValidationResult(Result.Forgotten, item));
					outValidationInfo.Succeeded = false;
				}
			}
		}
	}
}
