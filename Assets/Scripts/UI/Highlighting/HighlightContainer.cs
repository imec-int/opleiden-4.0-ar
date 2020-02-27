using System.Collections.Generic;
using UnityEngine;
using Data;
using Core;
using System.Text;
using System.Linq;
using TimeLineValidation;
using UnityEngine.Animations;

namespace UI.Highlighting
{
	public class HighlightContainer : MonoBehaviour
	{
		private GameObject _3DModel;

		[SerializeField]
		private Highlight _uiHighlightPrefab = null;

		[SerializeField]
		private InfoPanel _uiInfoPanel = null;

		[SerializeField]
		private ActionController _actionController = null;

		private List<Highlight> _uiHighlightInstanceList = new List<Highlight>();
		private HighlightAnchor[] _anchors = null;
		private List<GameObject> _consequenceObjects = new List<GameObject>();

		private bool _handlingConsequences = false;

		#region Monobehaviour
		protected void Awake()
		{
			_actionController.ValidationCompleted += OnValidationCompleted;
		}
		protected void OnEnable()
		{
			_3DModel = GameObject.FindGameObjectWithTag("Installation");
			Reset();
		}
		#endregion

		#region Interface
		public void Reset()
		{
			ClearHighlights();
			ClearConsequences();
			_handlingConsequences = false;
			PlaceHighlights(_3DModel);
		}

		// Enables/Disables visibility of all the highlights
		public void SetHighlightsVisibility(bool visible)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(visible);
			}
		}
		#endregion

		#region Methods
		private void ClearHighlights()
		{
			foreach (Highlight highlight in _uiHighlightInstanceList)
			{
				highlight.OnExpanded -= OnHighlightSelected;
				Destroy(highlight.gameObject);
			}
			_uiHighlightInstanceList.Clear();
		}

		private void PlaceHighlights(GameObject model)
		{
			_anchors = model.GetComponentsInChildren<HighlightAnchor>();
			Debug.Assert(_anchors.Length > 0, $"Failed to retrieve anchors for the provided gameobject {model.name}");

			foreach (HighlightAnchor anchor in _anchors)
			{
				Highlight newObj = Instantiate(_uiHighlightPrefab, anchor.transform.position, Quaternion.Euler(0, 0, 0), this.transform);
				newObj.Setup(anchor, OnInfoPanelRequested, _actionController);
				_uiHighlightInstanceList.Add(newObj);
				// Set up new object
				newObj.OnExpanded += OnHighlightSelected;

				PositionConstraint constraint = newObj.gameObject.AddComponent<PositionConstraint>();
				ConstraintSource constraintSrc = new ConstraintSource();
				constraintSrc.sourceTransform = anchor.transform;
			
				constraint.AddSource(constraintSrc);
				constraint.constraintActive = true;
			}
		}

		private void ClearConsequences()
		{
			foreach (GameObject consequence in _consequenceObjects)
			{
					Destroy(consequence);
			}
			_consequenceObjects.Clear();
		}

		private void SpawnConsequenceVisualisation(Transform transform, GameObject prefab)
		{
			if(!prefab) return;
			_consequenceObjects.Add(Instantiate(prefab,transform));
		}

		private void HandleConsequences(ValidationStageReport report)
		{
			Dictionary<HighlightAnchor, List<Operation>> anchorsFailedOpsDict = CollateAnchorsWithFailedOperations(report);
			var spawnedPrefabs = new List<GameObject>();
			foreach (HighlightAnchor anchor in anchorsFailedOpsDict.Keys)
			{
				foreach (ConsequenceData data in anchor.Consequences)
				{
					if(data.AssociatedOperation == Operation.None || anchorsFailedOpsDict[anchor].Contains(data.AssociatedOperation))
					{
						if (spawnedPrefabs.Contains(data.VisualizationPrefab))
							continue;
						SpawnConsequenceVisualisation(anchor.transform,data.VisualizationPrefab);
						spawnedPrefabs.Add(data.VisualizationPrefab);
					}
				}
				spawnedPrefabs.Clear();
			}
		}

		private static Dictionary<HighlightAnchor, List<Operation>> CollateAnchorsWithFailedOperations(ValidationStageReport report)
		{
			// Associate a part with its results
			var anchorsResultsDict = new Dictionary<HighlightAnchor, List<Operation>>();
			foreach (var result in report.ForgottenActionsValidationResult)
			{
				HighlightAnchor anchor = result.Action.Part.GetComponent<HighlightAnchor>();
				if (!anchorsResultsDict.ContainsKey(anchor))
					anchorsResultsDict.Add(anchor, new List<Operation>());
				anchorsResultsDict[anchor].Add(result.Action.Operation);
			}
			return anchorsResultsDict;
		}

		private void GetHighlightInfo(HighlightAnchor anchor, out string header, out string body)
		{
			StringBuilder strBuilder = new StringBuilder(anchor.Info.Body);
			if(_handlingConsequences)
			{
				foreach (ConsequenceData item in anchor.Consequences)
				{
					strBuilder.AppendFormat("<br><br>{0}", item.Body);
				}
			}
			header = anchor.Info.Header;
			body = strBuilder.ToString();
		}
		#endregion

		#region Callbacks
		private void OnHighlightSelected(Highlight sender)
		{
			// Collapse all non-selected highlights
			foreach (Highlight highlight in _uiHighlightInstanceList)
			{
				if (highlight != sender)
				{
					highlight.Collapse();
				}
			}
		}

		private void OnInfoPanelRequested(HighlightAnchor anchor)
		{
			SetHighlightsVisibility(false);
			GetHighlightInfo(anchor, out string header, out string body);
			_uiInfoPanel.Show(header,body);
			_uiInfoPanel.OnClose += OnInfoPanelClosed;
		}

		private void OnInfoPanelClosed()
		{
			SetHighlightsVisibility(true);
			_uiInfoPanel.OnClose -= OnInfoPanelClosed;
		}

		private void OnValidationCompleted(ValidationStageReport report)
		{
			_handlingConsequences = true;
			ClearConsequences();
			HandleConsequences(report);
		}
		#endregion

	}
}
