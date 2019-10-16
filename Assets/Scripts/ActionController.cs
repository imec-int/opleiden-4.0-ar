using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public enum ValidationResult
{
	None = 0,
	Correct,
	IncorrectPart,
	IncorrectOperation,
	IncorrectIndex,
	CompletelyIncorrect,
	Unnecessary,
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

	public override string ToString()
	{
		var results = ValidationResultList.Select(result => Enum.GetName(typeof(ValidationResult),result));
		var resultsAsString = string.Join(",", results);
		return $"Amount of errors: {AmountOfErrors}; Results list: {resultsAsString} ";
	}
}

public class ActionController : MonoBehaviour
{
	private List<ActionData> _Actions = new List<ActionData>();

	[SerializeField]
	private ValidationRuleSet _ValidationRuleSet;

	public ValidationInfo ValidationReport
	{
		get; private set;
	}

	public event Action<ActionData> ActionAdded, ActionUpdated, ActionDeleted;
	public event Action<ActionData, int> ActionMoved;
	public event Action<ValidationInfo> ValidationCompleted;

#region Monobehaviour
	void Awake()
	{
		Debug.Assert(_ValidationRuleSet.CheckIfValid(), "Current Validation Ruleset is not valid!!");
		_ValidationRuleSet.Setup();
		ValidationReport = new ValidationInfo();
	}
#endregion

#region Action Manipulation
	public void AddAction(ActionData action)
	{
		_Actions.Add(action);
		action.Index = _Actions.Count;

		ActionAdded?.Invoke(action);

		// TODO: REMOVE TEMPORARY CODE
		ValidateActions();
	}

	private void UpdateAction(ActionData action)
	{
		ActionUpdated?.Invoke(action);
	}

	public void DeleteAction(ActionData action)
	{
		ActionDeleted?.Invoke(action);

		_Actions.RemoveAt(action.Index - 1);

		for (int i = action.Index - 1; i < _Actions.Count; i++)
		{
			_Actions[i].Index = i + 1;
			UpdateAction(_Actions[i]);
		}
	}

	public void MovedAction(ActionData action, int newIndex)
	{
		// newIndex starts from 0, increment to match action indexes
		newIndex++;

		ActionMoved?.Invoke(action, newIndex);

		// Swap Action position in array
		_Actions.RemoveAt(action.Index - 1);
		_Actions.Insert(newIndex - 1, action);

		// Update all action indexes after the original or the new index
		for (int i = Mathf.Min(action.Index, newIndex) - 1; i < _Actions.Count; i++)
		{
			_Actions[i].Index = i + 1;
			UpdateAction(_Actions[i]);
		}
	}
#endregion

#region Action Validation
	public void ValidateActions()
	{
		bool succeeded = false;
		List<ValidationResult> results = new List<ValidationResult>();
		List<ActionData> checkList = _ValidationRuleSet.ActionsInOrderList;

		// First Pass: Missing/Unnecesarry/Operation/Part/Correct
		for(int i = 0; i < Math.Max(_Actions.Count,checkList.Count); ++i)
		{
			var result = ValidationResult.None;
			// Past action count
			if(i >= _Actions.Count)
			{
				result = ValidationResult.Missing;
			}
			else if (i >= checkList.Count)
			{
				result =ValidationResult.Unnecessary;
			}			
			else
				result = CheckPrimaryFailure(i);

			results.Add(result);
		}

		// Second Pass: Tolerance for positions
		for(int i = 0; i < results.Count; ++i)
		{
			if(results[i] == ValidationResult.Correct)
				continue;
		}

		// Set results
		ValidationReport = new ValidationInfo()
		{
			Succeeded = succeeded,
			ValidationResultList = results,
		};
		ValidationCompleted?.Invoke(ValidationReport);
		Debug.Log(ValidationReport);
	}

    private ValidationResult CheckPrimaryFailure(int index)
    {
		ActionData actionToValidate = _Actions[index];
		ActionData actionToCheckAgainst = _ValidationRuleSet.ActionsInOrderList.ElementAtOrDefault(index);
		if (actionToValidate == actionToCheckAgainst)
		{
			return ValidationResult.Correct;
		}
		else
		{
			if (actionToValidate.Part == actionToCheckAgainst?.Part)
			{
				return ValidationResult.IncorrectOperation;
			}
			else if (actionToValidate.Operation == actionToCheckAgainst?.Operation)
				return ValidationResult.IncorrectPart;
			else
				return ValidationResult.CompletelyIncorrect;
		}
    }
    #endregion
}
