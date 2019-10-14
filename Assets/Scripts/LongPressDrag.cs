using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class IntEvent : UnityEvent<int>
{
}


public class LongPressDrag : LongPressEventTrigger, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Transform _Parent;
	private ScrollRect _ScrollRect;
	private GameObject _Spacer;

	public IntEvent _OnIndexChanged = new IntEvent();

	private int _SiblingIndex;

	private bool isDragging = false;

	protected override void Awake()
	{
		base.Awake();
		_Parent = transform.parent;
		_ScrollRect = transform.GetComponentInParent<ScrollRect>();
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (isDragging)
		{
			GetComponent<RectTransform>().position = eventData.position;
			float sizeX = GetComponent<RectTransform>().sizeDelta.x;
			float offsetX = GetComponent<RectTransform>().anchoredPosition.x - sizeX / 2;

			float result = offsetX / sizeX;
			_Spacer.transform.SetSiblingIndex((int)result);
		}
		else
		{
			_ScrollRect.SendMessage("OnDrag", eventData);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (isDragging)
		{
			isDragging = false;
			transform.SetParent(_Parent);
			transform.SetSiblingIndex(_Spacer.transform.GetSiblingIndex());
			Destroy(_Spacer);

			if (_SiblingIndex != transform.GetSiblingIndex())
			{
				_OnIndexChanged.Invoke(transform.GetSiblingIndex());
			}
		}
		else
		{
			_ScrollRect.SendMessage("OnEndDrag", eventData);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (_IsPointerDown && _LongPressTriggered)
		{
			isDragging = true;
			_SiblingIndex = transform.GetSiblingIndex();
			_Spacer = new GameObject("Spacer");
			_Spacer.transform.SetParent(_Parent);
			_Spacer.AddComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			_Spacer.transform.SetSiblingIndex(_SiblingIndex);
			transform.SetParent(_ScrollRect.transform);
		}
		else
		{
			_ScrollRect.SendMessage("OnBeginDrag", eventData);
		}
	}
}
