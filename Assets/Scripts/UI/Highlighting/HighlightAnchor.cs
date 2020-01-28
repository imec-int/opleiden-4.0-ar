using Data;
using UnityEngine;

namespace UI.Highlighting
{
	public class HighlightAnchor : MonoBehaviour
	{
		[SerializeField]
		private Operation[] _AvailableOperations;

		[SerializeField]
		private HighlightInfo _Info;

		[SerializeField]
		private PartType _HighlightedPart;

		public Operation[] AvailableOperations { get => _AvailableOperations; }
		public HighlightInfo Info { get => _Info; }
		public PartType HighlightedPart { get => _HighlightedPart; }


#if UNITY_EDITOR
		private void Awake()
		{
			bool assertion = _Info != null && _HighlightedPart != PartType.None;
			Debug.Assert(assertion, $"HighlightAnchor: {this.name} is not set up correctly");
		}
#endif
	}
}
