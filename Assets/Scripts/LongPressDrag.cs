using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class UnityIntEvent : UnityEvent<int> { }

public class LongPressDrag : LongPressEventTrigger, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField]
	private GameObject _SpacerPrefab;

	private RectTransform _ParentRT, _RectTransform, _Spacer;
	private ScrollRect _ScrollRect;

	public UnityIntEvent _OnIndexChanged = new UnityIntEvent();

	private int _SiblingIndex;

	private bool _IsDragging = false;

	protected override void Awake()
	{
		base.Awake();
		_RectTransform = transform.GetComponent<RectTransform>();
		_ParentRT = _RectTransform.parent.GetComponent<RectTransform>();
		_ScrollRect = _RectTransform.GetComponentInParent<ScrollRect>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (_LongPressTriggered)
		{
			_IsDragging = true;
			_SiblingIndex = _RectTransform.GetSiblingIndex();
			_Spacer = GameObject.Instantiate(_SpacerPrefab, _ParentRT).GetComponent<RectTransform>();
			_Spacer.SetSiblingIndex(_SiblingIndex);
			_RectTransform.SetParent(_ScrollRect.transform);
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
			float sizeX = _RectTransform.sizeDelta.x;
			float offsetX = _RectTransform.anchoredPosition.x - sizeX / 2;

			float result = offsetX / sizeX;
			_Spacer.SetSiblingIndex((int)result);
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
