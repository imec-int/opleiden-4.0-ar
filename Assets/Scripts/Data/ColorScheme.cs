using UnityEngine;
using System;
using RotaryHeart.Lib.SerializableDictionary;
using TimeLineValidation;
using UnityEngine.UI;

namespace Data
{
	[Serializable] public class ValidationColorsDictionary : SerializableDictionaryBase<Result, ColorBlock> { }
	[Serializable] public class UIColorsDictionary : SerializableDictionaryBase<ColorStyleables, ColorBlock> { };

	public enum ColorStyleables
	{
		None,
		Button
	}

	// Class to assign colors to other values
	[CreateAssetMenu(fileName = "ColorData", menuName = "opleiden-4.0-ar/ColorScheme")]
	public class ColorScheme : ScriptableObject
	{
		[SerializeField]
		private ValidationColorsDictionary _validationColorDictionary = new ValidationColorsDictionary();

		[SerializeField]
		// Using another dictionary, otherwise Unity just dumps a color block with no indication what it is for
		private UIColorsDictionary _uiColorsDictionary = new UIColorsDictionary();

		// Default constructor with some dummy data for validation results
		public ColorScheme()
		{
			_validationColorDictionary.Add(Result.None, ColorBlock.defaultColorBlock);
			_validationColorDictionary.Add(Result.Correct, new ColorBlock());
			_validationColorDictionary.Add(Result.IncorrectPosition, new ColorBlock());
			_validationColorDictionary.Add(Result.Forgotten, new ColorBlock());
			_validationColorDictionary.Add(Result.Incorrect, new ColorBlock());

			_uiColorsDictionary.Add(ColorStyleables.Button, ColorBlock.defaultColorBlock);
		}

		public ValidationColorsDictionary ValidationColorDictionary => _validationColorDictionary;
		public UIColorsDictionary UIColorsDictionary => _uiColorsDictionary;
	}
}
