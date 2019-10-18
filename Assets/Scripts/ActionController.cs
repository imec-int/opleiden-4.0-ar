using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TimeLineValidation;

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

		// First Pass: Missing/Unnecesarry/Operation/Part/Correct/Displaced
		int errorCount = 0;
		for(int i = 0; i < Math.Max(_Actions.Count,checkList.Count); ++i)
		{
			var result = ValidationResult.None;
			// Over action count
			if(i >= _Actions.Count)
			{
				result = ValidationResult.Missing;
			}
			ActionData actionToValidate = _Actions.ElementAtOrDefault(i);
			ActionData actionToCheckAgainst = _ValidationRuleSet.ActionsInOrderList.ElementAtOrDefault(i-errorCount);

			if (actionToValidate != null)
			{
				result = actionToValidate.ValidateAgainst(actionToCheckAgainst);
				// Check for displacement
				if (result == ValidationResult.IncorrectIndex && errorCount > 0)
				{
					result = ValidationResult.Displaced;
				}
				
				if (result == ValidationResult.CompletelyIncorrect)
					++errorCount;
			}
			results.Add(result);
		}	

		/* 
		// Second Pass: Tolerance for positions
		for(int i = 0; i < results.Count; ++i)
		{
			ValidationResult currResult = results[i];
			if(currResult == ValidationResult.Correct
				|| currResult == ValidationResult.Missing
				|| currResult == ValidationResult.Unnecessary)
				continue;

			results[i] = CheckWithinTolerance(i,results);
		}
		*/

		// Set results
		ValidationReport = new ValidationInfo()
		{
			Succeeded = succeeded,
			ValidationResultList = results,
		};
		ValidationCompleted?.Invoke(ValidationReport);
		Debug.Log(ValidationReport);
	}
	private ValidationResult CheckWithinTolerance(int index, List<ValidationResult> otherResults)
	{
		ValidationResult result = otherResults[index];
		var toleranceRange = Enumerable.Range(Math.Max(index-_ValidationRuleSet.PositionTolerance,0),_ValidationRuleSet.PositionTolerance*2);
		foreach (int i in toleranceRange)
		{
			if (i == index)
				continue;
			if (i > otherResults.Count)
				break;

			ValidationResult checkAgainst = otherResults[i];
			// Ignore results that are already correct
			if (checkAgainst == ValidationResult.Correct)
				continue;

			if (_Actions[index].ValidateAgainst(_ValidationRuleSet.ActionsInOrderList[i]) == ValidationResult.IncorrectIndex)
				return ValidationResult.IncorrectIndex;
			
		}
		return result;
	}
    #endregion
}
