using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TimeLineValidation;
using Data;

namespace Core
{
	public class ActionController : MonoBehaviour
	{
		public ValidationInfo[] ValidationReports
		{
			get; private set;
		}

		public List<IndexedActionData> Actions { get; } = new List<IndexedActionData>();

		public event Action<IndexedActionData> ActionAdded, ActionUpdated, ActionDeleted;
		public event Action<IndexedActionData, int> ActionMoved;
		public event Action<ValidationStageReport> ValidationCompleted;

		public event Action PostReset;

		public void Reset()
		{
			Actions.Clear();
			PostReset?.Invoke();
		}

		#region Action Manipulation
		public void AddAction(IndexedActionData action)
		{
			Actions.Add(action);
			action.Index = Actions.Count;

			ActionAdded?.Invoke(action);
		}

		private void UpdateAction(IndexedActionData action)
		{
			ActionUpdated?.Invoke(action);
		}

		public void DeleteAction(IndexedActionData action)
		{
			ActionDeleted?.Invoke(action);

			Actions.RemoveAt(action.Index - 1);

			for (int i = action.Index - 1; i < Actions.Count; i++)
			{
				Actions[i].Index = i + 1;
				UpdateAction(Actions[i]);
			}
			//ValidateActions();
		}

		public void MovedAction(IndexedActionData action, int newIndex)
		{
			// newIndex starts from 0, increment to match action indexes
			newIndex++;

			ActionMoved?.Invoke(action, newIndex);

			// Swap Action position in array
			Actions.RemoveAt(action.Index - 1);
			Actions.Insert(newIndex - 1, action);

			// Update all action indexes after the original or the new index
			for (int i = Mathf.Min(action.Index, newIndex) - 1; i < Actions.Count; i++)
			{
				Actions[i].Index = i + 1;
				UpdateAction(Actions[i]);
			}
			//ValidateActions();
		}
		#endregion

		#region Action Validation
		public void ValidateActions()
		{
			ActionSequence[] actionSequences = FindObjectsOfType<ActionSequence>();
			ValidationReports = new ValidationInfo[actionSequences.Length];
			for (int i = 0; i < actionSequences.Length; i++)
			{
				List<ActionData> actions = Actions.Select(action => action as ActionData).ToList();
				actionSequences[i].Validate(actions, out ValidationReports[i]);
			}
			ValidationCompleted?.Invoke(GenerateStageReport(ValidationReports));
		}
		#endregion

		private ValidationStageReport GenerateStageReport(ValidationInfo[] validationReports)
		{
			ValidationStageReport report;
			report.RequiredActions = 0;
			report.PerformedActionsValidationResult = new List<ValidationResult>();
			report.ForgottenActionsValidationResult = new List<ValidationResult>();
			report.Succeeded = true;
			foreach (var validationInfo in validationReports)
			{
				report.RequiredActions += validationInfo.UsedRuleSet.ActionsCount;
				report.PerformedActionsValidationResult.AddRange(validationInfo.PerformedActionsValidationResult);
				report.ForgottenActionsValidationResult.AddRange(validationInfo.ForgottenActionsValidationResult);
				report.Succeeded &= validationInfo.Succeeded;
			}

			return report;
		}
	}

	public struct ValidationStageReport
	{
		public bool Succeeded;
		public uint RequiredActions;
		public List<ValidationResult> PerformedActionsValidationResult;
		public List<ValidationResult> ForgottenActionsValidationResult;
	}
}
