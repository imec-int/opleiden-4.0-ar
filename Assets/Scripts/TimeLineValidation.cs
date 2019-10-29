using System;
using System.Linq;
using System.Collections.Generic;

namespace TimeLineValidation
{
	public enum ValidationResult
	{
		None = 0,
		Correct,
		IncorrectPosition,
		Incorrect, // Not in the list of all validation steps 
	}

	public class ValidationInfo
	{
		public bool Succeeded { get; set; }
		public List<ValidationResult> ValidationResultList { get; set; }
		public List<int> ValidatedUIDs { get; set; }
		public ValidationRuleSet UsedRuleSet { get; set; }

		public int AmountOfErrors
		{
			get
			{
				return ValidationResultList.Count(result => result != ValidationResult.Correct);
			}
		}

		public override string ToString()
		{
			IEnumerable<string> results = ValidationResultList.Select(result => Enum.GetName(typeof(ValidationResult), result));
			string resultsAsString = string.Join(",", results);
			return $"Actions: {ValidationResultList.Count}/{UsedRuleSet.TotalStepCount};Amount of errors: {AmountOfErrors}; Results list: {resultsAsString} ";
		}
	}
}
