using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
	public class ActionToAnchorHighlighter : MonoBehaviour
	{
		private LineRenderer _lineRenderer;

		void Awake()
		{
			_lineRenderer = this.GetComponent<LineRenderer>();
			Debug.Assert(_lineRenderer != null, "Failed to find linerenderer component");
		}
		// Start is called before the first frame update
		void Start()
		{
			_lineRenderer.enabled = false;
			_lineRenderer.useWorldSpace = true;
		}

		// Update is called once per frame
		void Update()
		{

		}

		public Rect GetScreenCoordinates(RectTransform uiElement)
		{
			var worldCorners = new Vector3[4];
			uiElement.GetWorldCorners(worldCorners);
			var result = new Rect(
										worldCorners[0].x,
										worldCorners[0].y,
										worldCorners[2].x - worldCorners[0].x,
										worldCorners[2].y - worldCorners[0].y);
			return result;
		}

		public void ShowArrow(TimelineActionWidget widget, GameObject highlightAnchor)
		{
			ShowArrow(widget.transform.position, highlightAnchor.transform.position);
		}

		public void ShowArrow(Vector3 start, Vector3 end)
		{
			Vector3[] vecs = { start, end };
			_lineRenderer.SetPositions(vecs);
			_lineRenderer.enabled = true;
		}
	}
}
