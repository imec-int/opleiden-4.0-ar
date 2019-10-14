using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
	[SerializeField]
	private TimelineActionsView _TimelineActionsToolbar;

	private List<ActionData> _Actions = new List<ActionData>();

	public event Action<ActionData> ActionAdded, ActionUpdated, ActionDeleted;

	public void AddAction(ActionData action)
	{
		_Actions.Add(action);
		action.Index = _Actions.Count;

		ActionAdded?.Invoke(action);
	}

	private void UpdateAction(ActionData action)
	{
		ActionUpdated?.Invoke(action);
	}

	public void DeleteAction(ActionData action)
	{
		ActionDeleted?.Invoke(action);

		_Actions.RemoveAt(action.Index - 1);

		for (int i = action.Index - 1; i < _Actions.Count; i++)
		{
			_Actions[i].Index = i + 1;
			UpdateAction(_Actions[i]);
		}
	}
}
