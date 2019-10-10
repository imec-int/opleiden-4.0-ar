using UnityEngine;

public class ActionsBar : MonoBehaviour
{
	[SerializeField]
	private GameObject _Button;

	void Start()
	{
		for (int i = 0; i < 15; i++)
		{
			GameObject.Instantiate(_Button, transform);
		}
	}

}
