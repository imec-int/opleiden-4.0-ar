using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.Utilities
{
	public class OnRectTransformChanged : UIBehaviour
	{
		[SerializeField]
		private UnityEvent _changed = new UnityEvent();

		protected override void OnRectTransformDimensionsChange()
		{
			_changed.Invoke();
		}
	}
}
