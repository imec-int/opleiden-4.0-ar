using UnityEngine;

public class SetActiveOnAwake : MonoBehaviour
{
	[SerializeField]
	private GameObject _object;

	[SerializeField]
	private bool _active;

	private void Awake()
	{
		_object.SetActive(_active);
	}
}
