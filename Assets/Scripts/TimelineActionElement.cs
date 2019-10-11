using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimelineActionElement : MonoBehaviour
{
	[SerializeField]
	private ActionMetadata _ActionMetadata;

	[SerializeField]
	private TextMeshProUGUI _Index, _Icon, _Label;

	[SerializeField]
	private GameObject _CloseBtn;

	[SerializeField]
	private Graphic _OrderMarker;

	private Action _Action;

	public void Setup(Action action)
	{
		_Action = action;
		action.Update += ActionUpdated;

		UpdateState();
	}

	private void UpdateState()
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
			SetDelete(false);
		}
	}

	public void SetDelete(bool state)
	{
		_CloseBtn.SetActive(state);
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		_Action.Delete(_Action);
	}

	private void ActionUpdated(Action action)
	{
		UpdateState();
	}
}
