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
		action.Index = (uint)_Actions.Count;

		_TimelineActionsToolbar.AddTimelineAction(action);

		action.Delete += ActionDeleted;
	}

	private void ActionDeleted(ActionData action)
	{
		_Actions.RemoveAt((int)action.Index - 1);

		for (int i = (int)action.Index - 1; i < _Actions.Count; i++)
		{
			_Actions[i].Index = (uint)i + 1;
			_Actions[i].Update(_Actions[i]);
		}
	}
}
