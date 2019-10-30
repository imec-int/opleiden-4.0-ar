﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;
using UnityEngine.EventSystems;

namespace UI
{
	[RequireComponent(typeof(SphereCollider))]
	public class SphereButton : MonoBehaviour, IPointerClickHandler
	{
		private enum ButtonState
		{
			None = 0,
			Normal,
			Highlighted,
			Pressed,
			Selected,
			Disabled,
		}

		[SerializeField]
		private MeshRenderer[] _meshesToColor;

		[SerializeField]
		private ColorBlock _colors = ColorBlock.defaultColorBlock;

		[SerializeField]
		private float _clickFadeDuration = 0.2f;

		[SerializeField]
		private bool _selectAfterPress = true;

		private Dictionary<ButtonState, Color> _colorsDictionary;

		private int _colorShaderID;

		private ButtonState _currentState;

		private Color _wantedColor = Color.white;

		private Color _currentColor = Color.white;

		private float _tweenDuration;

		private float _elapsedTime;

		public ColorBlock Colors
		{
			get
			{
				return _colors;
			}
			set
			{
				_colors = value;
				FillColorsDictionary();
				StartColorTween();
			}
		}

		public bool Selected { get => _currentState == ButtonState.Pressed || _currentState == ButtonState.Selected; }

		public UnityEvent onClick;
		public event Action OnTweenFinished;

		#region Monobehaviour
		protected void Awake()
		{
			_colorShaderID = Shader.PropertyToID("_MainColor");
			FillColorsDictionary();
			_currentState = ButtonState.Normal;
			if (_selectAfterPress)
				OnTweenFinished += SelectAfterPress;
		}
		protected void Update()
		{
			TweenColor();
		}
		#endregion

		public void Deselect()
		{
			if (Selected)
				SetState(ButtonState.Normal);
		}

		public void Select()
		{
			if (!Selected)
				SetState(ButtonState.Selected);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (_currentState == ButtonState.Normal)
			{
				SetState(ButtonState.Pressed, _clickFadeDuration);
				onClick.Invoke();
			}
		}

		private void SetState(ButtonState newState)
		{
			_currentState = newState;
			StartColorTween();
		}

		private void SetState(ButtonState newState, float tweenDuration)
		{
			_currentState = newState;
			StartColorTween(tweenDuration);
		}

		private void FillColorsDictionary()
		{
			_colorsDictionary = new Dictionary<ButtonState, Color>
			{
				[ButtonState.Normal] = _colors.normalColor,
				[ButtonState.Highlighted] = _colors.highlightedColor,
				[ButtonState.Pressed] = _colors.pressedColor,
				[ButtonState.Selected] = _colors.selectedColor,
				[ButtonState.Disabled] = _colors.disabledColor,
			};
		}

		#region Tweening
		private void StartColorTween()
		{
			StartColorTween(_colorsDictionary[_currentState], _colors.fadeDuration);
		}

		private void StartColorTween(float duration)
		{
			StartColorTween(_colorsDictionary[_currentState], duration);
		}

		public void StartColorTween(Color targetColor, float duration)
		{
			_wantedColor = targetColor;
			_tweenDuration = duration;
			_elapsedTime = 0;
		}

		private void TweenColor()
		{
			if (_currentColor == _wantedColor)
			{
				OnTweenFinished?.Invoke();
				return;
			}
			float lerpVal = _elapsedTime.RemapValue(0, _tweenDuration, 0, 1);
			_currentColor = Color.Lerp(_currentColor, _wantedColor, lerpVal);
			_elapsedTime += Time.deltaTime;

			foreach (var renderer in _meshesToColor)
			{
				renderer.material.SetColor(_colorShaderID, _currentColor);
			}
		}

		private void SelectAfterPress()
		{
			if (_currentState == ButtonState.Pressed)
			{
				SetState(ButtonState.Selected);
			}
		}
		#endregion
	}
}