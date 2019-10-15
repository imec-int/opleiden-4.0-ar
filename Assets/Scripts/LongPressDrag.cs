using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class UnityIntEvent : UnityEvent<int> { }

public class LongPressDrag : LongPressEventTrigger, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Transform _Parent;
	private ScrollRect _ScrollRect;
	private GameObject _Spacer;

	public UnityIntEvent _OnIndexChanged = new UnityIntEvent();

	private int _SiblingIndex;

	private bool _IsDragging = false;

	protected override void Awake()
	{
		base.Awake();
		_Parent = transform.parent;
		_ScrollRect = transform.GetComponentInParent<ScrollRect>();
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (_IsDragging)
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
		if (_IsDragging)
		{
			_IsDragging = false;
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
		if (_LongPressTriggered)
		{
			_IsDragging = true;
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
