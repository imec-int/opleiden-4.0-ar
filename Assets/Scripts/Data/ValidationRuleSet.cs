using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Data;

namespace TimeLineValidation
{
	[Serializable]
	public class ActionPhase
	{
		[SerializeField]
		private string _name;

		[SerializeField]
		private bool _subStepOrderMatters = true;

		[SerializeField]
		private List<ActionData> _subSteps = new List<ActionData>();

		public int Index
		{
			get;
			set;
		}

		public List<int> SubstepUIDList
		{
			get
			{
				return _subSteps.Select(action => action.UID).ToList();
			}
		}

		public List<ActionData> SubSteps => _subSteps;
		public bool SubStepOrderMatters => _subStepOrderMatters;
	}

	[CreateAssetMenu(fileName = "ValidationRuleSet", menuName = "opleiden-4.0-ar/ValidationRuleSet", order = 1)]
	public class ValidationRuleSet : ScriptableObject
	{
		#region Unity Facing
		[SerializeField]
		private string _name;

		[SerializeField]
		private List<ActionPhase> _actionPhaseList;
		#endregion

		#region Internals for validation check
		#region Generated
		private Dictionary<int, int> _uidUsageInRulesetDict = new Dictionary<int, int>();
		public uint TotalStepCount { get; private set; }
		#endregion
		#region State management
		private Dictionary<int, int> _uidUsageInTimelineDict = new Dictionary<int, int>();
		private int _actionPhaseIndex;
		private int _subStepIndex;
		#endregion
		#endregion

		public bool Initialize()
		{
			bool result = true;
			TotalStepCount = 0;
			// collapse actions to a single list
			foreach (ActionPhase phase in _actionPhaseList)
			{
				foreach (ActionData step in phase.SubSteps)
				{
					// Check if action is correct
					if (step.Operation == Operation.None || step.Part == Part.None)
					{
						// We do not want this to fail to build a list of valid actions
						// this is intended for programmers to handle the error in the setup
						result = false;
						continue;
					}
					IncreaseUsageForUID(_uidUsageInRulesetDict, step.UID);
					++TotalStepCount;
				}
			}
			return result;
		}

		#region Runtime Validation
		public void Validate(List<ActionData> allTimelineActions, out ValidationInfo outValidationInfo)
		{
			// Set up validation info
			outValidationInfo = new ValidationInfo
			{
				ValidationResultList = new List<ValidationResult>(),
				ValidatedUIDs = new List<int>(),
				UsedRuleSet = this
			};

			// Reset
			_uidUsageInTimelineDict.Clear();
			_actionPhaseIndex = 0;
			_subStepIndex = 0;

			bool succeeded = true;
			// Go through all time line actions
			foreach (ActionData action in allTimelineActions)
			{
				ValidationResult result = ValidateAction(action);
				if (result != ValidationResult.Correct)
					succeeded = false;
				// Add result to list
				outValidationInfo.ValidationResultList.Add(result);
				outValidationInfo.ValidatedUIDs.Add(action.UID);
			}
			outValidationInfo.Succeeded = succeeded && TotalStepCount == allTimelineActions.Count;
		}

		private ValidationResult RecursivelyPhaseCheck(ActionData action)
		{
			ActionPhase currentActionPhase = _actionPhaseList.ElementAtOrDefault(_actionPhaseIndex);

			// If we go past the last phase, reset index to previous and exit;
			if (currentActionPhase == null)
			{
				--_actionPhaseIndex;
				_subStepIndex = 0;
				// default: wrong position // We already checked whether it's unnecessary
				return ValidationResult.IncorrectPosition;
			}
			if (currentActionPhase.SubstepUIDList.Contains(action.UID))
			{
				if (currentActionPhase.SubStepOrderMatters)
				{
					ActionData currentStep = currentActionPhase.SubSteps.ElementAtOrDefault(_subStepIndex);
					Debug.Assert(currentStep != null);
					// Increase substep
					++_subStepIndex;
					// Validate
					return currentStep.UID == action.UID ? ValidationResult.Correct : ValidationResult.IncorrectPosition;
				}
				else
				{
					return ValidationResult.Correct;
				}
			}
			else // Check next phase
			{
				++_actionPhaseIndex; _subStepIndex = 0;
				return RecursivelyPhaseCheck(action);
			}
		}

		private ValidationResult ValidateAction(ActionData action)
		{
			// Check unnecessary
			if (!_uidUsageInRulesetDict.ContainsKey(action.UID))
			{
				return ValidationResult.Incorrect;
			}
			// Check incorrect placement
			ValidationResult result = RecursivelyPhaseCheck(action);

			// Too many of the same UID check
			int count = IncreaseUsageForUID(_uidUsageInTimelineDict, action.UID);
			if (count > _uidUsageInRulesetDict[action.UID])
			{
				return ValidationResult.Incorrect;
			}
			return result;
		}
		#endregion

		#region Helpers
		private int IncreaseUsageForUID(Dictionary<int, int> dict, int UID)
		{
			if (dict.TryGetValue(UID, out int usage))
			{
				dict[UID] = ++usage;
			}
			else
				dict[UID] = 1;

			return dict[UID];
		}
		#endregion
	}
}
