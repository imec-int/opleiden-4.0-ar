using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class UnityIntEvent : UnityEvent<int> { }

[System.Serializable]
public class UnityPointerDragEvent : UnityEvent<PointerEventData> { }

public class LongPressDragAndDrop : LongPressEventTrigger, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField]
	private GameObject _SpacerPrefab;

	private RectTransform _ParentRT, _RectTransform, _Spacer;
	private ScrollRect _ScrollRect;

	public UnityIntEvent _OnIndexChanged = new UnityIntEvent();
	public UnityPointerDragEvent _OnDrag = new UnityPointerDragEvent();

	[SerializeField]
	private PointerEventData _LastDragEventData;

	private int _SiblingIndex;

	private bool _IsDragging = false;

	protected override void Awake()
	{
		base.Awake();
		_RectTransform = transform.GetComponent<RectTransform>();
		_ParentRT = _RectTransform.parent.GetComponent<RectTransform>();
		_ScrollRect = _RectTransform.GetComponentInParent<ScrollRect>();
	}

	protected override void Update()
	{
		base.Update();

		if (_IsDragging)
		{
			_OnDrag.Invoke(_LastDragEventData);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (_LongPressTriggered)
		{
			_IsDragging = true;
			_SiblingIndex = _RectTransform.GetSiblingIndex();
			_RectTransform.SetParent(_ScrollRect.transform);
			_Spacer = GameObject.Instantiate(_SpacerPrefab, _ParentRT).GetComponent<RectTransform>();
			_Spacer.SetSiblingIndex(_SiblingIndex);

			// Update content layout to avoid scrollrect resetting position
			LayoutRebuilder.ForceRebuildLayoutImmediate(_ParentRT);
		}
		else
		{
			_ScrollRect.SendMessage("OnBeginDrag", eventData);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (_IsDragging)
		{
			_RectTransform.position = eventData.position;
			float sizeX = _RectTransform.sizeDelta.x + 6;
			float offsetX = -(_ParentRT.anchoredPosition.x + _ParentRT.parent.GetComponent<RectTransform>().rect.width) + _RectTransform.anchoredPosition.x - sizeX / 2;

			float result = offsetX / sizeX;
			_Spacer.SetSiblingIndex((int)result);
			_LastDragEventData = eventData;

			// Update content layout to avoid scrollrect resetting position
			LayoutRebuilder.ForceRebuildLayoutImmediate(_ParentRT);
		}
		else
		{
			_ScrollRect.SendMessage("OnDrag", eventData);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (_IsDragging)
		{
			_IsDragging = false;
			_RectTransform.SetParent(_ParentRT);
			_RectTransform.SetSiblingIndex(_Spacer.GetSiblingIndex());
			Destroy(_Spacer.gameObject);

			if (_SiblingIndex != _RectTransform.GetSiblingIndex())
			{
				_OnIndexChanged.Invoke(_RectTransform.GetSiblingIndex());
			}
		}
		else
		{
			_ScrollRect.SendMessage("OnEndDrag", eventData);
		}
	}
}
