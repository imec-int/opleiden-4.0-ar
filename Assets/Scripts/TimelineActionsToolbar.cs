using UnityEngine;

public class TimelineActionsToolbar : MonoBehaviour
{
	[SerializeField]
	private GameObject _ButtonPrefab;

	void Start()
	{
		for (int i = 0; i < 15; i++)
		{
			GameObject.Instantiate(_ButtonPrefab, transform);
		}
	}

}
