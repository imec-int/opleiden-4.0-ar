using UnityEngine;

namespace Develop
{
	public class EditorCamera : MonoBehaviour
	{
#if UNITY_EDITOR
		[SerializeField]
		private Vector3 _startPosition;

		[SerializeField]
		private Vector3 _startRotation;

		[SerializeField]
		private GameObject _installation;

		private void Awake()
		{
			_installation.SetActive(true);
			transform.SetPositionAndRotation(_startPosition, Quaternion.Euler(_startRotation));
			Destroy(this);
		}
#endif
	}
}
