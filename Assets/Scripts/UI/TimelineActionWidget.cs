using Core;
using Data;
using TimeLineValidation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(Button))]
	public class TimelineActionWidget : ActionWidget
	{
		[SerializeField]
		private TextMeshProUGUI _index;

		[SerializeField]
		private GameObject _closeBtn;

		[SerializeField]
		private Graphic _orderMarker;

		[SerializeField]
		private ColorScheme _colorScheme;

		private TextMeshProUGUI[] _foregroundTextObjects;
		private Color _defaultForegroundColor;

		private IndexedActionData _action;
		private ActionController _actionController;
		private Button _btnObj;
		private Image _bgImage;

		protected void Awake()
		{
			_btnObj = this.GetComponent<Button>();
			_bgImage = this.GetComponent<Image>();
			_foregroundTextObjects = this.GetComponentsInChildren<TextMeshProUGUI>();
			if (_foregroundTextObjects.Length > 0)
			{
				_defaultForegroundColor = _foregroundTextObjects[0].color;
			}
		}

		public void Setup(IndexedActionData action, ActionController controller, bool includeInValidation = true)
		{
			_actionController = controller;
			if (includeInValidation) _actionController.ValidationCompleted += VisualizeValidation;
			_action = action;
			UpdateState();
		}

		protected void OnDestroy()
		{
			_actionController.ValidationCompleted -= VisualizeValidation;
		}

		private void VisualizeValidation(ValidationStageReport report)
		{
			// Get result for this instance
			Result result = report.PerformedActionsValidationResult[_action.Index - 1].Result;
			bool valid = _colorScheme.ValidationColorDictionary.TryGetValue(result, out ColorBlock requiredColors);
			Debug.Assert(valid, $"Missing a color in color scheme for {result}");
			// Set the visuals
			SetBGColor(requiredColors);
			SetFGColor(result);

			if (result == Result.Correct)
			{
				SetDeleteBtnActive(false);
			}
		}

		private void SetBGColor(ColorBlock requiredColors)
		{
			_btnObj.colors = requiredColors;
			_bgImage.CrossFadeColor(requiredColors.normalColor, 0.5f, true, true);
		}

		private void SetFGColor(Result result)
		{
			foreach (var text in _foregroundTextObjects)
			{
				text.color = result == Result.None ? _defaultForegroundColor : Color.white;
			}
		}

		public void UpdateState()
		{
			_index.text = _action.Index.ToString();
			_orderMarker.color = Color.clear;
			base.Setup(_action.Operation, _action.PartType);
		}

		public void SetDeleteBtnActive(bool state)
		{
			_closeBtn.SetActive(state);
		}

		public void Delete()
		{
			_actionController.ValidationCompleted -= VisualizeValidation;
			_actionController.DeleteAction(_action);
		}

		public void Moved(int newIndex)
		{
			_actionController.MovedAction(_action, newIndex);
		}
	}
}
