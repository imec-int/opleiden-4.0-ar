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

	[SerializeField]
	private PointerEventData _LastDragEventData;

	public UnityIntEvent _OnIndexChanged = new UnityIntEvent();
	public UnityPointerDragEvent _OnDrag = new UnityPointerDragEvent();

	private RectTransform _ParentRT, _RectTransform, _SpacerRT;
	private ScrollRect _ScrollRect;

	private int _StartingIndex;
	private float _Spacing;
	private bool _IsDragging = false;

	protected override void Awake()
	{
		base.Awake();
		_RectTransform = transform.GetComponent<RectTransform>();
		_ParentRT = _RectTransform.parent.GetComponent<RectTransform>();
		_ScrollRect = _ParentRT.GetComponentInParent<ScrollRect>();

		_Spacing = _ParentRT.GetComponent<HorizontalOrVerticalLayoutGroup>().spacing;
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
			_StartingIndex = _RectTransform.GetSiblingIndex();
			_RectTransform.SetParent(_ScrollRect.transform);
			_SpacerRT = GameObject.Instantiate(_SpacerPrefab, _ParentRT).GetComponent<RectTransform>();
			_SpacerRT.SetSiblingIndex(_StartingIndex);

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
			float sizeX = _RectTransform.sizeDelta.x + _Spacing;
			float offsetX = -(_ParentRT.anchoredPosition.x + _ParentRT.parent.GetComponent<RectTransform>().rect.width) + _RectTransform.anchoredPosition.x - sizeX / 2;

			float result = offsetX / sizeX;
			_SpacerRT.SetSiblingIndex((int)result);
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
			int newIndex = _SpacerRT.GetSiblingIndex();
			Destroy(_SpacerRT.gameObject);
			_RectTransform.SetParent(_ParentRT);
			_RectTransform.SetSiblingIndex(newIndex);

			if (_StartingIndex != newIndex)
			{
				_OnIndexChanged.Invoke(newIndex);
			}
		}
		else
		{
			_ScrollRect.SendMessage("OnEndDrag", eventData);
		}
	}
}
