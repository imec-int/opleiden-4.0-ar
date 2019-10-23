using UnityEngine;
using System;
using RotaryHeart.Lib.SerializableDictionary;
using TimeLineValidation;
using UnityEngine.UI;

namespace Data
{
	[Serializable] public class ValidationColorsDictionary : SerializableDictionaryBase<ValidationResult,ColorBlock> {}

	// Class to assing colors to other values
	[CreateAssetMenu(fileName="ColorData", menuName="opleiden-4.0-ar/ColorScheme")]
	public class ColorScheme: ScriptableObject
	{
		[SerializeField]
		private ValidationColorsDictionary _validationColorDictionary = new ValidationColorsDictionary();

		[SerializeField]
		private Color _neutralColor = Color.white;

		private ColorBlock _defaultButtonColors = ColorBlock.defaultColorBlock;

		// Default constructor with some dummy data for validation results
		public ColorScheme()
		{
			ValidationColorDictionary.Add(ValidationResult.Correct,new ColorBlock());
			ValidationColorDictionary.Add(ValidationResult.Incorrect, new ColorBlock());
			ValidationColorDictionary.Add(ValidationResult.IncorrectPosition, new ColorBlock());
		}

		public ValidationColorsDictionary ValidationColorDictionary { get => _validationColorDictionary; set => _validationColorDictionary = value; }
		public Color NeutralColor { get => _neutralColor; set => _neutralColor = value; }
		public ColorBlock DefaultButtonColors { get => _defaultButtonColors; set => _defaultButtonColors = value; }
	}
}