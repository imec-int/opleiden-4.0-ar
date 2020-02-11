using System.Collections.Generic;
using UnityEngine;
using Data;
using Core;
using System.Text;

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
			_consequenceObjects.Add(Instantiate(prefab,transform,true));
		}

		private void HandleConsequences(ValidationStageReport report)
		{
			foreach (var validationResult in report.ForgottenActionsValidationResult)
			{
				HighlightAnchor anchor = validationResult.Action.Part.GetComponent<HighlightAnchor>();
				foreach (ConsequenceData consequenceData in anchor?.Consequences)
				{
					if(consequenceData.AssociatedOperation == Operation.None || consequenceData.AssociatedOperation == validationResult.Action.Operation)
					{
						SpawnConsequenceVisualisation(anchor.transform, consequenceData.VisualizationPrefab);
					}
				}
			}
		}

		private void GetHighlightInfo(HighlightAnchor anchor, out string header, out string body)
		{
			StringBuilder strBuilder = new StringBuilder(anchor.Info.Body);
			if(_handlingConsequences)
			{
				foreach (ConsequenceData item in anchor.Consequences)
				{
					strBuilder.AppendFormat("<br>{0}", item.Body);
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
