using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TimeLineValidation;
using Data;
using Utilities;
using Core;

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

		private readonly List<int> _possibleUIDList = new List<int>();

		public void Setup(HighlightAnchor anchor, Action<HighlightInfo> showHighlightInfo, ActionController controller)
		{
			AssociatedAnchor = anchor;

			_secondaryMenu.Setup(anchor.AvailableOperations, anchor.HighlightedPart,
			() => showHighlightInfo?.Invoke(anchor.Info), controller, this);

			_secondaryMenu.gameObject.SetActive(false);
			_sphereButton.onClick.AddListener(OnButtonClicked);

			// Get some possible UIDs we need to watch out for post validation
			foreach (var operation in anchor.AvailableOperations)
			{
				_possibleUIDList.Add(ActionData.CalculateUID(operation, anchor.HighlightedPart));
			}
			controller.ValidationCompleted += PostValidationVisualisation;
		}

		private void PostValidationVisualisation(ValidationInfo info)
		{
			// default color
			var worstResult = ValidationResult.None;

			// Pick worst colour to display
			for (int i = 0; i < info.ValidationResultList.Count; ++i)
			{
				if (!_possibleUIDList.Contains(info.ValidatedUIDs[i]))
					continue;

				ValidationResult matchingResult = info.ValidationResultList[i];
				if ((int)matchingResult > (int)worstResult)
					worstResult = matchingResult;

				if (worstResult == ValidationResult.Incorrect)
					break;
			}

			ColorBlock colors = _colorScheme.ValidationColorDictionary[worstResult];
			_sphereButton.Colors = colors;
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
