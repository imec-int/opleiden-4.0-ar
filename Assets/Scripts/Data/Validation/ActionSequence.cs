using UnityEngine;
using System.Collections.Generic;
using Data;
using UnityEngine.Assertions;
using System.Linq;

namespace TimeLineValidation
{
	public abstract class ActionSequence : MonoBehaviour
	{
		[SerializeField]
		protected List<ActionData> _actions;

		[SerializeField]
		protected bool _allowDuplicateActions;

		public uint ActionsCount { get { return (uint)_actions.Count; } }

		public virtual void Validate(List<ActionData> timelineActions, out ValidationInfo outValidationInfo)
		{
			// Set up validation info
			outValidationInfo = new ValidationInfo
			{
				PerformedActionsValidationResult = new List<ValidationResult>(),
				ForgottenActionsValidationResult = new List<ValidationResult>(),
				UsedRuleSet = this
			};
		}

		protected void OnValidate()
		{
			foreach (var action in _actions)
			{
				Assert.AreNotEqual(action.Operation, Operation.None, "The action does not have an operation assigned");
				Assert.AreNotEqual(action.PartType, PartType.None, "The action does not have a part type assigned");
				Assert.IsNotNull(action.Part, "The action does not have a part assigned");
			}
		}
	}
}
