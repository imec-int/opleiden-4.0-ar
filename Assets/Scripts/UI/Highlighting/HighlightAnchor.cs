using System.Collections.Generic;
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

		[SerializeField, Tooltip("Optional list of consequences")]
		private List<ConsequenceData> _consequences;

		public Operation[] AvailableOperations { get => _AvailableOperations; }
		public HighlightInfo Info { get => _Info; }
		public PartType HighlightedPart { get => _HighlightedPart; }

		public List<ConsequenceData> Consequences {get => _consequences;}

#if UNITY_EDITOR
		private void Awake()
		{
			bool assertion = _Info != null && _HighlightedPart != PartType.None;
			Debug.Assert(assertion, $"HighlightAnchor: {this.name} is not set up correctly");
		}
#endif
	}
}
