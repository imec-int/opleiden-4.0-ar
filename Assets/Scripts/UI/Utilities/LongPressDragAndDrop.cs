using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Utilities
{
	[System.Serializable]
	public class UnityIntEvent : UnityEvent<int> { }

	[System.Serializable]
	public class UnityPointerDragEvent : UnityEvent<PointerEventData> { }

	public class LongPressDragAndDrop : LongPressEventTrigger, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		[SerializeField]
		private GameObject _SpacerPrefab;

		[SerializeField]
		private PointerEventData _LastDragEventData;

		public UnityIntEvent _OnIndexChanged = new UnityIntEvent();
		public UnityPointerDragEvent _OnBeginDrag = new UnityPointerDragEvent();
		public UnityPointerDragEvent _OnDrag = new UnityPointerDragEvent();
		public UnityPointerDragEvent _OnEndDrag = new UnityPointerDragEvent();

		private RectTransform _parentRT, _rectTransform, _spacerRT;
		private ScrollRect _scrollRect;

		private int _startingIndex;
		private float _spacing;
		private bool _isDragging = false;

		protected override void Awake()
		{
			base.Awake();
			_rectTransform = transform.GetComponent<RectTransform>();
			_parentRT = _rectTransform.parent.GetComponent<RectTransform>();
			_scrollRect = _parentRT.GetComponentInParent<ScrollRect>();

			_spacing = _parentRT.GetComponent<HorizontalOrVerticalLayoutGroup>().spacing;
		}

		protected override void Update()
		{
			base.Update();

			if (_isDragging)
			{
				_OnDrag.Invoke(_LastDragEventData);
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (_LongPressTriggered)
			{
				_isDragging = true;
				_startingIndex = _rectTransform.GetSiblingIndex();
				_rectTransform.SetParent(_scrollRect.transform);
				_spacerRT = GameObject.Instantiate(_SpacerPrefab, _parentRT).GetComponent<RectTransform>();
				_spacerRT.SetSiblingIndex(_startingIndex);

				_OnBeginDrag.Invoke(eventData);

				// Update content layout to avoid scrollrect resetting position
				LayoutRebuilder.ForceRebuildLayoutImmediate(_parentRT);
			}
			else
			{
				_scrollRect.SendMessage("OnBeginDrag", eventData);
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (_isDragging)
			{
				_rectTransform.position = eventData.position;
				float sizeX = _rectTransform.sizeDelta.x + _spacing;
				float offsetX = -(_parentRT.anchoredPosition.x + _parentRT.parent.GetComponent<RectTransform>().rect.width) + _rectTransform.anchoredPosition.x - (sizeX / 2);

				float result = offsetX / sizeX;
				_spacerRT.SetSiblingIndex((int)result);
				_LastDragEventData = eventData;

				// Update content layout to avoid scrollrect resetting position
				LayoutRebuilder.ForceRebuildLayoutImmediate(_parentRT);
			}
			else
			{
				_scrollRect.SendMessage("OnDrag", eventData);
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (_isDragging)
			{
				_isDragging = false;
				int newIndex = _spacerRT.GetSiblingIndex();
				Destroy(_spacerRT.gameObject);
				_rectTransform.SetParent(_parentRT);
				_rectTransform.SetSiblingIndex(newIndex);

				if (_startingIndex != newIndex)
				{
					_OnIndexChanged.Invoke(newIndex);
				}

				_OnEndDrag.Invoke(eventData);
			}
			else
			{
				_scrollRect.SendMessage("OnEndDrag", eventData);
			}
		}
	}
}
