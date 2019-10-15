using UnityEngine;

public class ActionPart : MonoBehaviour
{
	[SerializeField]
	private Part _Part;

	[SerializeField]
	private ActionController _Manager;



	public void AddAction(int operation)
	{
		ActionData action = new ActionData();

		action.Operation = (Operation)operation;
		action.Part = _Part;
		_Manager.AddAction(action);
	}
}
