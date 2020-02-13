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

		public Button AssociatedButton
		{
			get { return _AssociatedButton; }
		}

		public void Setup(Operation operation, PartType partType)
		{
			string actionIcon = _ActionMetadata.ActionOperationsInfo[operation].Icon;
			actionIcon = actionIcon != "" ? actionIcon : _ActionMetadata.ActionPartsInfo[partType].Icon;
			Setup(_ActionMetadata.ActionOperationsInfo[operation].Name + " " + _ActionMetadata.ActionPartsInfo[partType].Name,
			actionIcon);

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
	}
}
