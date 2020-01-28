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
					if (!_allowDuplicateActions)
					{
						ActionData firstOfType = timelineActions.First(y => y.GetHashCode() == item.GetHashCode());
						outValidationInfo.PerformedActionsValidationResult.Add(new ValidationResult(timelineActions.Count(x => x.GetHashCode() == item.GetHashCode() && item != firstOfType) > 1 ? Result.Incorrect : Result.Correct, item));
					}
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
				}
			}
		}
	}
}
