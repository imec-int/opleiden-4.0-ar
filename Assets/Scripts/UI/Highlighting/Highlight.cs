using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TimeLineValidation;
using Data;
using Utilities;
using Core;
using System.Linq;

namespace UI.Highlighting
{
	[RequireComponent(typeof(LookAtCamera))]
	public class Highlight : MonoBehaviour
	{
		// Unity-facing variables
		// Useful children
		[SerializeField]
		private SphereButton _sphereButton = null;

		[SerializeField]
		private HighlightSecondaryMenu _secondaryMenu = null;

		[SerializeField]
		private TextMeshProUGUI[] _actionPositionLabels = new TextMeshProUGUI[4];

		[SerializeField]
		private ColorScheme _colorScheme;

		public event Action<Highlight> OnExpanded;
		public event Action<Highlight> OnCollapsed;

		public HighlightAnchor AssociatedAnchor
		{
			get; private set;
		}

		public bool Selected
		{
			get; private set;
		}

		private ActionController _actionController;

		public void Setup(HighlightAnchor anchor, Action<HighlightInfo> showHighlightInfo, ActionController controller)
		{
			AssociatedAnchor = anchor;
			gameObject.name = anchor.name;

			_secondaryMenu.Setup(anchor.AvailableOperations, anchor.HighlightedPart,
			() => showHighlightInfo?.Invoke(anchor.Info), controller, this);

			_secondaryMenu.gameObject.SetActive(false);
			_sphereButton.onClick.AddListener(OnButtonClicked);

			_actionController = controller;
			_actionController.ValidationCompleted += PostValidationVisualisation;
		}

		protected void OnDestroy()
		{
			_actionController.ValidationCompleted -= PostValidationVisualisation;
		}

		private void PostValidationVisualisation(ValidationStageReport report)
		{
			// default color
			var worstResult = Result.None;

			IEnumerable<Result> performedActionResults = from rep in report.PerformedActionsValidationResult
														 where rep.Action.Part.name == gameObject.name
														 select rep.Result;

			IEnumerable<Result> forgottenActionResults = from rep in report.ForgottenActionsValidationResult
														 where rep.Action.Part.name == gameObject.name
														 select rep.Result;

			foreach (var result in performedActionResults.Concat(forgottenActionResults))
			{
				if ((int)result > (int)worstResult)
					worstResult = result;

				if (worstResult == Result.Incorrect)
					break;
			}

			_sphereButton.Colors = _colorScheme.ValidationColorDictionary[worstResult];
		}

		#region Submenu handling
		public void Collapse()
		{
			_secondaryMenu.gameObject.SetActive(false);
			Selected = false;
			OnCollapsed?.Invoke(this);
			_sphereButton.Deselect();
		}

		// Event for the main button on this highlight
		private void OnButtonClicked()
		{
			Expand();
			OnExpanded?.Invoke(this);
			//_sphereButton.Select();
		}

		private void Expand()
		{
			//Show the menu
			_secondaryMenu.gameObject.SetActive(true);
			Selected = true;
		}
		#endregion
	}
}
