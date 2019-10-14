using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
	[SerializeField]
	private TimelineActionsView _TimelineActionsToolbar;

	private List<ActionData> _Actions = new List<ActionData>();

	public void AddAction(ActionData action)
	{
		_Actions.Add(action);
		action.Index = _Actions.Count;

		_TimelineActionsToolbar.ActionAdded(action);
	}

	private void UpdateAction(ActionData action)
	{
		_TimelineActionsToolbar.ActionUpdated(action);
	}

	public void DeleteAction(ActionData action)
	{
		_TimelineActionsToolbar.ActionDeleted(action);

		_Actions.RemoveAt(action.Index - 1);

		for (int i = action.Index - 1; i < _Actions.Count; i++)
		{
			_Actions[i].Index = i + 1;
			UpdateAction(_Actions[i]);
		}
	}

	public void MovedAction(ActionData action, int newIndex)
	{
		_Actions.RemoveAt(action.Index - 1);
		if (newIndex > action.Index) newIndex--;
		_Actions.Insert(newIndex - 1, action);

		_TimelineActionsToolbar.ActionMoved(action, newIndex);

		for (int i = Mathf.Min(action.Index, newIndex) - 1; i < _Actions.Count; i++)
		{
			_Actions[i].Index = i + 1;
			UpdateAction(_Actions[i]);
		}
	}
}
