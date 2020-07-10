using UnityEngine;

namespace UI
{
	public class ActionToAnchorHighlighter : MonoBehaviour
	{
		[SerializeField]
		float _width = 3.0f;

		[SerializeField]
		float _visibilityDuration = 1.5f;

		[SerializeField]
		AnimationCurve _alphaCurve;
		[SerializeField]
		AnimationCurve _scalingCurve;

		[SerializeField]
		private Texture _texture;

		private TimelineActionWidget _widget;
		private GameObject _anchor;

		private bool _showing;
		private float _timerStart;
		private float _animationProgress;

		// Lerps between two vector values
		Vector2 Vector2Lerp(Vector2 vecA, Vector2 vecB, float lerp)
		{
			var x = Mathf.Lerp(vecA.x, vecB.x, lerp);
			var y = Mathf.Lerp(vecA.y, vecB.y, lerp);

			return new Vector2(x, y);
		}

		void OnGUI()
		{
			if (!_showing || _widget == null || _anchor == null) return;

			// Create a rect, with its pivot at the top of the widget
			// It's as long as the distance between the widget and the anchor in screen space (pixels)
			Vector2 widgetScreenPos = GetScreenPositionFromTransform(_widget.GetComponent<RectTransform>());
			Vector2 anchorScreenPos = CalculateScreenPosFromWorldPos(_anchor.transform.position);
			Vector2 endPos = Vector2Lerp(widgetScreenPos, anchorScreenPos, _scalingCurve.Evaluate(_animationProgress));
			Vector2 fromWidgetToEnd = widgetScreenPos - endPos;

			// Rotate to face the end point
			var rotAngle = CalculateAngleToYAxis(fromWidgetToEnd.normalized);
			GUIUtility.RotateAroundPivot(rotAngle, widgetScreenPos);

			// Scale and draw
			float length = fromWidgetToEnd.magnitude;
			Rect drawRect = new Rect(widgetScreenPos, new Vector2(_width, length * -1));

			var color = _widget.UsedColors.normalColor;
			color.a = _alphaCurve.Evaluate(_animationProgress);
			Debug.Log("Progress:" + _animationProgress + "\nAlpha" + _alphaCurve.Evaluate(_animationProgress));
			GUI.DrawTexture(drawRect, _texture, ScaleMode.StretchToFill, true, 0, color, 0, 0);
		}

		void Update()
		{
			if (!_showing)
				return;
			_animationProgress = (Time.time - _timerStart) / _visibilityDuration;
			// Auto-hide the arrow after {visibletime}
			if (_animationProgress >= 1)
				_showing = false;
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
			// because we offset the bottom, we need to offset the top
			screenPoint.x -= _width * 0.5f;

			Vector3 heading = position - Camera.main.transform.position;
			if (Vector3.Dot(Camera.main.transform.forward, heading) < 0)
			{
				screenPoint = -screenPoint;
			}

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
			_showing = true;
			_timerStart = Time.time;
		}

		public void Hide()
		{
			_showing = false;
		}
	}
}
