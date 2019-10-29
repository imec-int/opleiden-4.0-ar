using UnityEngine;

namespace Utilities
{
	public class LookAtCamera : MonoBehaviour
	{
		private RectTransform _RectTransform = null;
		private Camera _MainCamera = null;

		private void Awake()
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
}
