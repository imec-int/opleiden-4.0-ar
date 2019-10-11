using UnityEngine;

public class ActionPart : MonoBehaviour
{
	[SerializeField]
	private Part _Part;

	[SerializeField]
	private ActionManager _Manager;



	public void AddAction(int operation)
	{
		Action action = new Action();

		action.Operation = (Operation)operation;
		action.Part = _Part;
		_Manager.AddAction(action);
	}
}
