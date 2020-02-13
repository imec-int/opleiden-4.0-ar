using UnityEngine;

namespace Data
{
	[CreateAssetMenu(fileName="ConsequenceData", menuName="opleiden-4.0-ar/ConsequenceData", order=4)]
	public class ConsequenceData: ScriptableObject
	{

		[SerializeField, Tooltip("Extra info to append to the info panel"), MultilineAttribute(5)]
		private string _extraInfo;

		[SerializeField, Tooltip("Optional prefab to spawn")]
		private GameObject _visualizationPrefab;

		[SerializeField, Tooltip("Optional operation to which this consequence is specific")]
		private Operation _associatedOperation;

		public string Body { get => _extraInfo; }
		public GameObject VisualizationPrefab {get => _visualizationPrefab;}

		public Operation AssociatedOperation {get => _associatedOperation;}
	}
}