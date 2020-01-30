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
			Setup(_ActionMetadata.ActionOperationsInfo[operation].Name + " " + _ActionMetadata.ActionPartsInfo[partType].Name,
			 _ActionMetadata.ActionPartsInfo[partType].Icon);
		}

		public void Setup(string label, string icon)
		{
			_Label.text = label;
			_Icon.text = icon.ToUnicodeForTMPro();
		}
	}
}
