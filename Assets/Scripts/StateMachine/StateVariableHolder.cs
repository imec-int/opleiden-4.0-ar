using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace StateMachine
{
	public enum Widget
	{
		LoadingScreen,
		TimeLine,
		PumpTag,
		HighlightContainer,
		TimelineTopper,
	}

	public class StateVariableHolder : MonoBehaviour
	{
		[Serializable] public class WidgetDictionary : SerializableDictionaryBase<Widget, GameObject> { }

		[SerializeField]
		private WidgetDictionary _widgets;

		public WidgetDictionary Widgets { get => _widgets; }
	}
}
