using UnityEngine;

public class ActionPart : MonoBehaviour
{
	[SerializeField]
	private Part _Part;

	[SerializeField]
	private ActionController _Manager;



	public void AddAction(int operation)
	{
		IndexedActionData action = new IndexedActionData();

		action.Operation = (Operation)operation;
		action.Part = _Part;
		_Manager.AddAction(action);
	}
}
