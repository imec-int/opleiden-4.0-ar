using UnityEngine;

public class ActionsBar : MonoBehaviour
{
	[SerializeField]
	private GameObject m_Button;

	void Start()
	{
		for (int i = 0; i < 15; i++)
		{
			GameObject.Instantiate(m_Button, transform);
		}
	}

}
