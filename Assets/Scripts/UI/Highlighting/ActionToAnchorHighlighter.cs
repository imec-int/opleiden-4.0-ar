using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
	public class ActionToAnchorHighlighter : MonoBehaviour
	{
		[SerializeField]
		float _width = 3.0f;

		private LineRenderer _lineRenderer;
		private TimelineActionWidget _widget;
		private GameObject _anchor;

		[SerializeField]
		private Texture _texture;

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
		void OnGUI()
		{
			if (_widget != null && _anchor != null)
			{
				Rect currentWidgetDrawPos = GetGUIRectFromRectTransform(_widget.GetComponent<RectTransform>());
				Vector2 screenPosAnchor = CalculateScreenPosFromWorldPos(_anchor.transform.position);
				float length = Vector2.Distance(currentWidgetDrawPos.position, screenPosAnchor);
				currentWidgetDrawPos.height = -1 * length;
				// rotate to face
				GUI.Box(new Rect(screenPosAnchor, new Vector2(10.0f, 10.0f)), "");
				var rotAngle = CalculateRotationAngle(currentWidgetDrawPos.position, screenPosAnchor);
				GUIUtility.RotateAroundPivot(rotAngle, currentWidgetDrawPos.position);
				GUI.DrawTexture(currentWidgetDrawPos, _texture);
			}
		}

		#region  OnGUI Helpers
		private static readonly int TOP_RIGHT_CORNER = 2;
		private static readonly int TOP_LEFT_CORNER = 1;
		Rect GetGUIRectFromRectTransform(RectTransform rectTransform)
		{
			var corners = new Vector3[4];
			rectTransform.GetWorldCorners(corners);
			// Halfway between the top left and top right
			var x = Mathf.Lerp(corners[TOP_LEFT_CORNER].x, corners[TOP_RIGHT_CORNER].x, 0.5f);
			var y = Mathf.Lerp(corners[TOP_LEFT_CORNER].y, corners[TOP_RIGHT_CORNER].y, 0.5f);
			// Flip the Y-Coordinate... Because Unity cannot decide whether Y is up or down.
			var startPos = new Vector2(x, Screen.height - y);
			;
			return new Rect(startPos, new Vector2(_width, 1.0f));
		}

		Vector2 CalculateScreenPosFromWorldPos(Vector3 position)
		{
			Vector2 screenPoint = Camera.main.WorldToScreenPoint(position);
			screenPoint.y = Screen.height - screenPoint.y;
			return screenPoint;
		}
		float CalculateRotationAngle(Vector2 widgetPos, Vector2 anchorPos)
		{
			var toPos = (widgetPos - anchorPos).normalized;
			var upAxis = Vector2.up;
			// var dotP = Vector2.Dot(Vector2.up, toPos);
			return Mathf.Rad2Deg * (Mathf.Atan2(toPos.y, toPos.x) - Mathf.Atan2(Vector2.up.y, Vector2.up.x));
		}
		#endregion
		public void ShowArrow(TimelineActionWidget widget, GameObject highlightAnchor)
		{
			_widget = widget;
			_anchor = highlightAnchor;
			// ShowArrow(widget.transform.position, highlightAnchor.transform.position);
			// _showing = true;
		}

		public void ShowArrow(Vector3 start, Vector3 end)
		{
			Vector3[] vecs = { start, end };
			_lineRenderer.SetPositions(vecs);
			_lineRenderer.enabled = true;
		}
	}
}
