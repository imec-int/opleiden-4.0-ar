using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;

namespace UI
{
	[RequireComponent(typeof(SphereCollider))]
	public class SphereButton : MonoBehaviour
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
		
		private Dictionary<ButtonState, Color> _colorsDictionary;

		private int _colorShaderID;

		private ButtonState _currentState;

		private Color _wantedColor = Color.white;

		private Color _currentColor = Color.white;

		private float _tweenDuration;

		private float _elapsedTime;

		private float _alphaModulator = 1.0f;

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

		public bool Selected { get => _currentState == ButtonState.Pressed || _currentState == ButtonState.Selected;} 

		public UnityEvent onClick;
		public event Action OnTweenFinished;

#region Monobehaviour

		public void Deselect()
		{
			if(Selected)
				SetState(ButtonState.Normal);
		}

		protected void Awake()
		{
			_colorShaderID = Shader.PropertyToID("_MainColor");
			FillColorsDictionary();
			_currentState = ButtonState.Normal;
			OnTweenFinished += TweenFinished;
		}

		private void TweenFinished()
		{
			if (_currentState == ButtonState.Pressed)
			{
				SetState(ButtonState.Selected);
			}
		}

		protected void OnMouseEnter()
		{
			if(_currentState == ButtonState.Normal)
			{
				SetState(ButtonState.Highlighted);
			}
		}

		protected void OnMouseDown()
		{
			if (_currentState == ButtonState.Highlighted)
			{
				_currentState = ButtonState.Pressed;
				StartColorTween(_clickFadeDuration);
				onClick.Invoke();
			}
		}

		protected void OnMouseExit()
		{
			if (_currentState == ButtonState.Highlighted)
			{
				SetState(ButtonState.Normal);
			}	
		}

		protected void Update()
		{
			TweenColor();
		}
#endregion

		private void SetState(ButtonState newState)
		{
			_currentState = newState;
			StartColorTween();
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
			float lerpVal = _elapsedTime.RemapValue(0,_tweenDuration,0,1);
			_currentColor = Color.Lerp(_currentColor, _wantedColor, lerpVal);
			_elapsedTime += Time.deltaTime;

			foreach(var renderer in _meshesToColor)
			{
				renderer.material.SetColor(_colorShaderID,_currentColor);
			}
		}
#endregion
	}
}