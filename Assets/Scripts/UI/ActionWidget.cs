using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
	public class ActionWidget : MonoBehaviour
	{
		[SerializeField]
		protected ActionMetadata _ActionMetadata;

		[SerializeField]
		protected TextMeshProUGUI _Icon, _Label;

		[SerializeField]
		protected Button _AssociatedButton;

		public bool IconFlickering
		{
			get;
			set;
		}

		private float _startTime;
		private Color _iconColor;
		private float _iconStartAlpha;

		public Button AssociatedButton
		{
			get { return _AssociatedButton; }
		}

		public void Awake()
		{
			_iconColor = _Icon.color;
			_iconStartAlpha = _iconColor.a;
		}

		public void Setup(Operation operation, PartType partType)
		{
			string actionIcon = _ActionMetadata.ActionOperationsInfo[operation].Icon;
			actionIcon = actionIcon != "" ? actionIcon : _ActionMetadata.ActionPartsInfo[partType].Icon;
			Setup(_ActionMetadata.ActionOperationsInfo[operation].Name + " " + _ActionMetadata.ActionPartsInfo[partType].Name,
			actionIcon);
		}

		protected void OnEnable()
		{
			_startTime = Time.time;
		}

		protected void OnDisable()
		{
			_iconColor.a = _iconStartAlpha;
			_Icon.color = _iconColor;
		}

		public void Setup(string label, string icon)
		{
			_Label.text = label;
			SetIcon(icon);
		}

		public void SetIcon(string icon)
		{
			_Icon.text = icon.ToUnicodeForTMPro();
		}

		protected virtual void Update()
		{
			if (IconFlickering)
			{
				_iconColor.a = Mathf.Lerp(0.33f, 1, Mathf.Cos((_startTime - Time.time) * 5) + 1 * 0.5f);
				_Icon.color = _iconColor;
			}
		}
	}
}
