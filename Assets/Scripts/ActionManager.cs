using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
	[SerializeField]
	private TimelineActionsToolbar _TimelineActionsToolbar;

	private List<Action> _Actions = new List<Action>();

	public void AddAction(Action action)
	{
		_Actions.Add(action);
		action.Index = (uint)_Actions.Count;

		_TimelineActionsToolbar.AddTimelineAction(action);

		action.Delete += ActionDeleted;
	}

	private void ActionDeleted(Action action)
	{
		_Actions.RemoveAt((int)action.Index - 1);

		for (int i = (int)action.Index - 1; i < _Actions.Count; i++)
		{
			_Actions[i].Index = (uint)i + 1;
			_Actions[i].Update(_Actions[i]);
		}
	}
}
