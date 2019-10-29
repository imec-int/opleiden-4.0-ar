using UnityEngine;

namespace Data
{
	[CreateAssetMenu(fileName = "HighlightInfo", menuName = "opleiden-4.0-ar/HighlightInfo", order = 1)]
	public class HighlightInfo : ScriptableObject
	{
		[SerializeField]
		private string _Header;

		[SerializeField]
		private string _Body;

		public string Header { get => _Header; }
		public string Body { get => _Body; }
	}
}
