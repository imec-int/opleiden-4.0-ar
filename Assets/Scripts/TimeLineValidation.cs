using System;
using System.Linq;
using System.Collections.Generic;
using Data;

namespace TimeLineValidation
{
	public enum Result
	{
		None = 0,
		Correct,
		IncorrectPosition,
		Forgotten,
		Incorrect // Not in the list of all validation steps 
	}

	public struct ValidationResult
	{
		public Result Result;
		public ActionData Action;

		public ValidationResult(Result result, ActionData action)
		{
			Result = result;
			Action = action;
		}
	}

	public struct ValidationInfo
	{
		public bool Succeeded { get; set; }
		public List<ValidationResult> PerformedActionsValidationResult { get; set; }
		public List<ValidationResult> ForgottenActionsValidationResult;
		public ActionSequence UsedRuleSet { get; set; }

		public int AmountOfErrors
		{
			get
			{
				return PerformedActionsValidationResult.Count(result => result.Result != Result.Correct);
			}
		}

		public override string ToString()
		{
			IEnumerable<string> results = PerformedActionsValidationResult.Select(result => Enum.GetName(typeof(Result), result.Result));
			string resultsAsString = string.Join(",", results);
			return $"Actions: {PerformedActionsValidationResult.Count}/{UsedRuleSet.ActionsCount};Amount of errors: {AmountOfErrors}; Results list: {resultsAsString} ";
		}
	}
}
