using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TimeLineValidation;
using Data;

[RequireComponent(typeof(LookAtCamera))]
public class UIHighlight : MonoBehaviour
{
	// Unity-facing variables
	// Useful children
	[SerializeField]
	private Button _mainButton = null;
	[SerializeField]
	private UIHighlightSecondaryMenu _secondaryMenu = null;
	[SerializeField]
	private TextMeshProUGUI[] _actionPositionLabels = new TextMeshProUGUI[4];

	[SerializeField]
	private ColorScheme _colorScheme;

	public event Action<UIHighlight> OnExpanded;

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
		() =>
		{
			showHighlightInfo?.Invoke(anchor.Info);
		}, controller, this);

		_secondaryMenu.gameObject.SetActive(false);
		_mainButton.onClick.AddListener(OnButtonClicked);

		// Get some possible UIDs we need to watch out for post validation
		foreach(Operation operation in anchor.AvailableOperations)
		{
			_possibleUIDList.Add(ActionData.CalculateUID(operation, anchor.HighlightedPart));
		}
		controller.ValidationCompleted += PostValidationVisualisation;
	}

    private void PostValidationVisualisation(ValidationInfo info)
    {
		// default color
		var colors = _colorScheme.UIColorsDictionary[ColorStyleables.Button];
		var worstResult = ValidationResult.None;

		// Pick worst colour to display
		for (int i = 0; i < info.ValidationResultList.Count; ++i)
		{
			if(!_possibleUIDList.Contains(info.ValidatedUIDs[i]))
				continue;

			var matchingResult = info.ValidationResultList[i];
			if ((int)matchingResult > (int)worstResult)
				worstResult = matchingResult;

			if (worstResult == ValidationResult.Incorrect)
				break;
		}

		if (worstResult != ValidationResult.None)
			colors = _colorScheme.ValidationColorDictionary[worstResult];

		if (_mainButton != null)
			_mainButton.colors = colors;
    }
#region Submenu handling
    public void Collapse()
	{
		_secondaryMenu.gameObject.SetActive(false);
		Selected = false;
		// TODO: [PLDN-51] Color change
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
		// TODO: Color change
	}
#endregion
}