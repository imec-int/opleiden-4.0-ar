using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace TimeLineValidation
{
	public enum ValidationResult
	{
		None = 0,
		Correct,
		IncorrectPosition,
		Unnecessary, // Not in the list of all validation steps
		Missing,
	}
	public class ValidationInfo
	{
		public bool Succeeded;
		public List<ValidationResult> ValidationResultList;
		public int AmountOfErrors
		{
			get
			{
				return ValidationResultList.Count(result => result != ValidationResult.Correct);
			}
		}

		public int AmountOfRequiredActions { get; set; }

		public override string ToString()
		{
			var results = ValidationResultList.Select(result => Enum.GetName(typeof(ValidationResult), result));
			var resultsAsString = string.Join(",", results);
			return $"Amount of errors: {AmountOfErrors}; Results list: {resultsAsString} ";
		}
	}
}