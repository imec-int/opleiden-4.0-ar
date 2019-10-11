using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
	[SerializeField]
	private TimelineActionsToolbar _TimelineActionsToolbar;

	private List<Action> _Actions = new List<Action>();

	public void AddAction(Action action)
	{
		action.Index = (uint)_Actions.Count;
		_Actions.Add(action);

		_TimelineActionsToolbar.AddTimelineAction(action);
	}
}
