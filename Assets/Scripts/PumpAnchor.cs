using UnityEngine;

public class PumpAnchor : MonoBehaviour
{
	[SerializeField]
	private string _pumpID;

	public string PumpID { get => _pumpID; }

#if UNITY_EDITOR
	void Awake()
	{
		bool assertion = PumpID.Length == 0;
		Debug.Assert(!assertion, $"{name}: is not set up correctly");
	}
#endif
}
