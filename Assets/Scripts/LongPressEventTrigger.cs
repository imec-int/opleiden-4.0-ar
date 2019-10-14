using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongPressEventTrigger : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	[SerializeField]
	private float _LongPressThreshold = 1.0f;

	public UnityEvent _OnLongPress = new UnityEvent();

	protected bool _IsPointerDown, _LongPressTriggered;
	private float _TimePressed;

	protected virtual void Update()
	{
		if (_IsPointerDown && !_LongPressTriggered)
		{
			_TimePressed += Time.deltaTime;

			if (_TimePressed > _LongPressThreshold)
			{
				StartLongPress();
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_IsPointerDown = true;
		_LongPressTriggered = false;
		_TimePressed = 0;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_IsPointerDown = false;
		_LongPressTriggered = false;
		_TimePressed = 0;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_IsPointerDown = false;
		_LongPressTriggered = false;
		_TimePressed = 0;
	}

	protected virtual void StartLongPress()
	{
		_LongPressTriggered = true;
		_OnLongPress.Invoke();
		_TimePressed = 0;
	}
}
