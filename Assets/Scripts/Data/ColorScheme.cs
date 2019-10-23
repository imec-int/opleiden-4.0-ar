using UnityEngine;
using System;
using RotaryHeart.Lib.SerializableDictionary;
using TimeLineValidation;
using UnityEngine.UI;

namespace Data
{
	[Serializable] public class ValidationColorsDictionary : SerializableDictionaryBase<ValidationResult,ColorBlock> {}
	[Serializable] public class UIColorsDictionary: SerializableDictionaryBase<ColorStyleables, ColorBlock>{};

	public enum ColorStyleables
	{
		None,
		Button
	}

	// Class to assing colors to other values
	[CreateAssetMenu(fileName="ColorData", menuName="opleiden-4.0-ar/ColorScheme")]
	public class ColorScheme: ScriptableObject
	{
		[SerializeField]
		private ValidationColorsDictionary _validationColorDictionary = new ValidationColorsDictionary();

		[SerializeField]
		// Using another dictionary, otherwise Unity just dumps a color block with no indication what it is for
		private UIColorsDictionary _uiColorsDictionary = new UIColorsDictionary();

		[SerializeField]
		private Color _neutralColor = Color.white;

		// Default constructor with some dummy data for validation results
		public ColorScheme()
		{
			ValidationColorDictionary.Add(ValidationResult.Correct,new ColorBlock());
			ValidationColorDictionary.Add(ValidationResult.Incorrect, new ColorBlock());
			ValidationColorDictionary.Add(ValidationResult.IncorrectPosition, new ColorBlock());

			_uiColorsDictionary.Add(ColorStyleables.Button,ColorBlock.defaultColorBlock);
		}

        public ValidationColorsDictionary ValidationColorDictionary => _validationColorDictionary;
        public Color NeutralColor => _neutralColor;
		public UIColorsDictionary UIColorsDictionary => _uiColorsDictionary;
    }
}