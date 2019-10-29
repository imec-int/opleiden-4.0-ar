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

		private IndexedActionData _action;
		private ActionController _actionController;
		private Button _btnObj;
		private Image _bgImage;

		private void Awake()
		{
			_btnObj = this.GetComponent<Button>();
			_bgImage = this.GetComponent<Image>();
		}

		public void Setup(IndexedActionData action, ActionController controller)
		{
			_actionController = controller;
			_actionController.ValidationCompleted += VisualizeValidation;
			_action = action;
			UpdateState();
		}

		private void VisualizeValidation(ValidationInfo info)
		{
			// Get result for this instance
			ValidationResult result = info.ValidationResultList[_action.Index - 1];
			bool valid = _colorScheme.ValidationColorDictionary.TryGetValue(result, out ColorBlock requiredColors);
			Debug.Assert(valid, $"Missing a color in color scheme for {result}");
			// Set the visuals
			SetBGColor(requiredColors);
		}

		private void SetBGColor(ColorBlock requiredColors)
		{
			_btnObj.colors = requiredColors;
			_bgImage.CrossFadeColor(requiredColors.normalColor, 0.5f, true, true);
		}

		public void UpdateState()
		{
			_index.text = _action.Index.ToString();
			_orderMarker.color = Color.clear;
			base.Setup(_action.Operation, _action.Part);
		}

		private void Update()
		{
			// TODO: Input.GetMouseButtonDown(0) does this work on mobile?
			if (_closeBtn.activeSelf && Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject != _closeBtn)
			{
				SetDeleteBtnActive(false);
			}
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
