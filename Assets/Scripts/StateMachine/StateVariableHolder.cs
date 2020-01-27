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
		ARModeSelectionMenu
	}

	public enum Component
	{
		TrackingController,
		InfoPanel
	}

	public class StateVariableHolder : MonoBehaviour
	{
		[Serializable] public class WidgetDictionary : SerializableDictionaryBase<Widget, GameObject> { }

		[Serializable] public class ComponentDictionary : SerializableDictionaryBase<Component, MonoBehaviour> { }

		[SerializeField]
		private WidgetDictionary _widgets;

		public WidgetDictionary Widgets { get => _widgets; }

		[SerializeField]
		private ComponentDictionary _components;

		public ComponentDictionary Components { get => _components; }
	}
}
