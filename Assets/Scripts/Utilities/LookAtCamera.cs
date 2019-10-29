using UnityEngine;

namespace Utilities
{
	public class LookAtCamera : MonoBehaviour
	{
		private RectTransform _rectTransform = null;
		private Camera _mainCamera = null;

		private void Awake()
		{
			_rectTransform = this.GetComponent<RectTransform>();
			_mainCamera = Camera.main;
		}

		private void Update()
		{
			_rectTransform.LookAt(_mainCamera.transform);
			_rectTransform.forward *= -1;
		}
	}
}
