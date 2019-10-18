using UnityEngine;

public class MoveToTransform : MonoBehaviour
{
	[SerializeField]
	private Transform _transform;

	private void Awake()
	{
		_transform.SetPositionAndRotation(transform.position, transform.rotation);
	}
}
