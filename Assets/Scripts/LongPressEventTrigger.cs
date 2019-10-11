using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongPressEventTrigger : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	[SerializeField]
	private float _LongPressThreshold = 1.0f;

	public UnityEvent _OnLongPress = new UnityEvent();

	private bool _IsPointerDown, _LongPressTriggered;
	private float _TimePressed;

	private void Update()
	{
		if (_IsPointerDown && !_LongPressTriggered)
		{
			_TimePressed += Time.deltaTime;

			if (_TimePressed > _LongPressThreshold)
			{
				_LongPressTriggered = true;
				_OnLongPress.Invoke();
				_TimePressed = 0;
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_IsPointerDown = true;
		_LongPressTriggered = false;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_IsPointerDown = false;
		_LongPressTriggered = false;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_IsPointerDown = false;
		_LongPressTriggered = false;
	}
}
