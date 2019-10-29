using UnityEngine;

namespace Develop
{
	public class PumpAnchor : MonoBehaviour
	{
		[SerializeField]
		private string _pumpID;

		public string PumpID { get => _pumpID; }

#if UNITY_EDITOR
		private void Awake()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(_pumpID), $"{name}: is not set up correctly");
		}
#endif
	}
}
