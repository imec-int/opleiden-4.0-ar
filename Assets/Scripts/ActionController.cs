using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TimeLineValidation;

public class ActionController : MonoBehaviour
{
	private List<IndexedActionData> _actions = new List<IndexedActionData>();

	[SerializeField]
	private ValidationRuleSet _validationRuleSet;

	public ValidationInfo ValidationReport
	{
		get; private set;
	}

	public event Action<IndexedActionData> ActionAdded, ActionUpdated, ActionDeleted;
	public event Action<IndexedActionData, int> ActionMoved;
	public event Action<ValidationInfo> ValidationCompleted;

#region Monobehaviour
	void Awake()
	{
		bool rulesetCorrect = _validationRuleSet.Initialize();
		Debug.Assert(rulesetCorrect, "Current Validation Ruleset contains invalid substeps!!");
		ValidationReport = new ValidationInfo();
	}
#endregion

#region Action Manipulation
	public void AddAction(IndexedActionData action)
	{
		_actions.Add(action);
		action.Index = _actions.Count;

		ActionAdded?.Invoke(action);

		// TODO: REMOVE TEMPORARY CODE
		ValidateActions();
	}

	private void UpdateAction(IndexedActionData action)
	{
		ActionUpdated?.Invoke(action);
	}

	public void DeleteAction(IndexedActionData action)
	{
		ActionDeleted?.Invoke(action);

		_actions.RemoveAt(action.Index - 1);

		for (int i = action.Index - 1; i < _actions.Count; i++)
		{
			_actions[i].Index = i + 1;
			UpdateAction(_actions[i]);
		}

		// TODO: REMOVE TEMPORARY CODE
		ValidateActions();
	}

	public void MovedAction(IndexedActionData action, int newIndex)
	{
		// newIndex starts from 0, increment to match action indexes
		newIndex++;

		ActionMoved?.Invoke(action, newIndex);

		// Swap Action position in array
		_actions.RemoveAt(action.Index - 1);
		_actions.Insert(newIndex - 1, action);

		// Update all action indexes after the original or the new index
		for (int i = Mathf.Min(action.Index, newIndex) - 1; i < _actions.Count; i++)
		{
			_actions[i].Index = i + 1;
			UpdateAction(_actions[i]);
		}

		// TODO: REMOVE TEMPORARY CODE
		ValidateActions();
	}
#endregion

#region Action Validation
	public void ValidateActions()
	{
		_validationRuleSet.Validate(_actions.Select(action => action as ActionData).ToList(), out ValidationInfo reportCard);
		// report on the report
		ValidationReport = reportCard;
		ValidationCompleted?.Invoke(ValidationReport);
		Debug.Log(ValidationReport);
	}
#endregion
}
