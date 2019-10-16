using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimelineActionWidget : UIActionElement
{
	[SerializeField]
	private TextMeshProUGUI _Index;

	[SerializeField]
	private GameObject _CloseBtn;

	[SerializeField]
	private Graphic _OrderMarker;

	private ActionData _Action;
	private ActionController _ActionController;

	public void Setup(ActionData action, ActionController controller)
	{
		_ActionController = controller;
		_Action = action;

		UpdateState();
	}

	public void UpdateState()
	{
		_Index.text = _Action.Index.ToString();
		_OrderMarker.color = Color.clear;
		base.Setup(_Action.Operation,_Action.Part);
	}

	private void Update()
	{
		// TODO: Input.GetMouseButtonDown(0) does this work on mobile?
		if (_CloseBtn.activeSelf && Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject != _CloseBtn)
		{
			SetDeleteBtnActive(false);
		}
	}

	public void SetDeleteBtnActive(bool state)
	{
		_CloseBtn.SetActive(state);
	}

	public void Delete()
	{
		_ActionController.DeleteAction(_Action);
	}

	public void Moved(int newIndex)
	{
		_ActionController.MovedAction(_Action, newIndex);
	}
}
