using UnityEngine;

namespace UI
{
	public class ActionToAnchorHighlighter : MonoBehaviour
	{
		[SerializeField]
		float _width = 3.0f;

		[SerializeField]
		float visibleTime = 0.5f;

		[SerializeField]
		private Texture _texture;

		private TimelineActionWidget _widget;
		private GameObject _anchor;

		private bool _showing;

		void OnGUI()
		{
			if (_widget != null && _anchor != null)
			{
				// Create a rect, with its pivot at the top of the widget
				// It's as long as the distance between the widget and the anchor in screen space (pixels)
				Vector2 widgetScreenPos = GetScreenPositionFromTransform(_widget.GetComponent<RectTransform>());
				Vector2 anchorScreenPos = CalculateScreenPosFromWorldPos(_anchor.transform.position);
				Vector2 fromWidgetToAnchorDir = widgetScreenPos - anchorScreenPos;

				// Rotate to face the end point
				var rotAngle = CalculateAngleToYAxis(fromWidgetToAnchorDir.normalized);
				GUIUtility.RotateAroundPivot(rotAngle, widgetScreenPos);

				// Scale and draw
				float length = fromWidgetToAnchorDir.magnitude;
				Rect drawRect = new Rect(widgetScreenPos, new Vector2(_width, length * -1));
				GUI.DrawTexture(drawRect, _texture);
			}
		}

		#region  OnGUI Helpers
		private static readonly int TOP_RIGHT_CORNER = 2;
		private static readonly int TOP_LEFT_CORNER = 1;
		Vector2 GetScreenPositionFromTransform(RectTransform rectTransform)
		{
			var corners = new Vector3[4];
			rectTransform.GetWorldCorners(corners);
			// Halfway between the top left and top right
			var x = Mathf.Lerp(corners[TOP_LEFT_CORNER].x, corners[TOP_RIGHT_CORNER].x, 0.5f) - (_width * 0.5f);
			var y = Mathf.Lerp(corners[TOP_LEFT_CORNER].y, corners[TOP_RIGHT_CORNER].y, 0.5f);
			// Flip the Y-Coordinate... Because Unity cannot decide whether Y is up or down.
			return new Vector2(x, Screen.height - y);
		}

		Vector2 CalculateScreenPosFromWorldPos(Vector3 position)
		{
			Vector2 screenPoint = Camera.main.WorldToScreenPoint(position);
			screenPoint.y = Screen.height - screenPoint.y;
			return screenPoint;
		}
		float CalculateAngleToYAxis(Vector2 normalizedDirVec)
		{
			return Mathf.Rad2Deg * (Mathf.Atan2(normalizedDirVec.y, normalizedDirVec.x) - Mathf.Atan2(Vector2.up.y, Vector2.up.x));
		}
		#endregion
		public void ShowArrow(TimelineActionWidget widget, GameObject highlightAnchor)
		{
			_widget = widget;
			_anchor = highlightAnchor;
			// ShowArrow(widget.transform.position, highlightAnchor.transform.position);
			// _showing = true;
		}

		public void Hide()
		{
			_showing = false;
		}
	}
}
