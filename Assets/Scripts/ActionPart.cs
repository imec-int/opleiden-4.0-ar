using UnityEngine;

public class ActionPart : MonoBehaviour
{
	[SerializeField]
	private Part _Part;

	[SerializeField]
	private ActionManager _Manager;

	Action _Action = new Action();

	public void AddAction(int operation)
	{
		_Action.Operation = (Operation)operation;
		_Action.Part = _Part;
		_Manager.AddAction(_Action);
	}
}
