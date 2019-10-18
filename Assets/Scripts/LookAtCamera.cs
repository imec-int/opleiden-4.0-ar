using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	private RectTransform _RectTransform = null;
	private Camera _MainCamera = null;

	void Awake()
	{
		_RectTransform = this.GetComponent<RectTransform>();
		_MainCamera = Camera.main;
	}

	private void Update()
	{
		_RectTransform.LookAt(_MainCamera.transform);
		_RectTransform.forward *= -1;
	}
}
