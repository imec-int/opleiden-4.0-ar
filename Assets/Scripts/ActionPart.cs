using UnityEngine;

public class ActionPart : MonoBehaviour
{
	[SerializeField]
	private Part _Part;

	[SerializeField]
	private ActionManager _Manager;

	Action _Action = new Action();

	public void AddAction(int handling)
	{
		_Action.Handling = (Handling)handling;
		_Action.Part = _Part;
		_Manager.AddAction(_Action);
	}
}
