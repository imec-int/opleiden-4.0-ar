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
		[SerializeField]
		private ValidationRuleSet _validationRuleSet;

		public ValidationInfo ValidationReport
		{
			get; private set;
		}

		public List<IndexedActionData> Actions { get; } = new List<IndexedActionData>();

		public event Action<IndexedActionData> ActionAdded, ActionUpdated, ActionDeleted;
		public event Action<IndexedActionData, int> ActionMoved;
		public event Action<ValidationInfo> ValidationCompleted;

		public event Action PostReset;

		public void Reset()
		{
			Actions.Clear();
			PostReset?.Invoke();
		}

		#region Monobehaviour
		protected void Awake()
		{
			bool rulesetCorrect = _validationRuleSet.Initialize();
			Debug.Assert(rulesetCorrect, "Current Validation Ruleset contains invalid substeps!!");
			ValidationReport = new ValidationInfo();
		}
		#endregion

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
			_validationRuleSet.Validate(Actions.Select(action => action as ActionData).ToList(), out ValidationInfo reportCard);
			// report on the report
			ValidationReport = reportCard;
			ValidationCompleted?.Invoke(ValidationReport);
			//Debug.Log(ValidationReport);
		}
		#endregion
	}
}
