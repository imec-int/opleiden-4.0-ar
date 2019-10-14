using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimelineActionWidget : MonoBehaviour
{
	[SerializeField]
	private ActionMetadata _ActionMetadata;

	[SerializeField]
	private TextMeshProUGUI _Index, _Icon, _Label;

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
		_Label.text = _ActionMetadata.ActionOperationsInfo[_Action.Operation].Name + " " + _ActionMetadata.ActionPartsInfo[_Action.Part].Name;
		_OrderMarker.color = Color.clear;
		_Icon.text = _ActionMetadata.ActionPartsInfo[_Action.Part].Icon;
	}

	private void Update()
	{
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
}
