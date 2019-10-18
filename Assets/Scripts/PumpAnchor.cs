using UnityEngine;

public class PumpAnchor : MonoBehaviour
{
	[SerializeField]
	private string _pumpID;

	public string PumpID { get => _pumpID; }

#if UNITY_EDITOR
	void Awake()
	{
		Debug.Assert(!string.IsNullOrWhiteSpace(_pumpID), $"{name}: is not set up correctly");
	}
#endif
}
