using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Data;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using System.Collections;

namespace UI
{
	public class InfoPanel : MonoBehaviour, IPointerClickHandler
	{
		public event Action OnOpen;
		public event Action OnClose;

		[SerializeField]
		private TextMeshProUGUI _headerLabel;

		[SerializeField]
		private TextMeshProUGUI _bodyLabel;

		[SerializeField]
		private Button _closeButton;

		[SerializeField]
		private Scrollbar _verticalScrollBar;

		private bool _tapToClose;
		private int _infoHash;

		private Stack<GameObject> _temporaryObjects = new Stack<GameObject>();

		public void Awake()
		{
			_closeButton.onClick.AddListener(Close);
			_bodyLabel.gameObject.SetActive(false);
		}

		// This is necessary to call from Unity editor
		public void Show(HighlightInfo info)
		{
			Show(info.Header, info.Body);
		}

		public void Show(HighlightInfo info, bool showCloseBtn = true, bool tapToClose = false)
		{
			Show(info.Header, info.Body, showCloseBtn, tapToClose);
		}

		public void Show(string header, string body, bool showCloseBtn = true, bool tapToClose = false)
		{
			this.gameObject.SetActive(true);
			Canvas.ForceUpdateCanvases();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());

			// Check if the content has changed by doing a hash comparison. If unchanged do an early return
			int infoHash = (header + body).GetHashCode();
			if (infoHash == _infoHash) return;
			_infoHash = infoHash;

			_tapToClose = tapToClose;

			OnOpen?.Invoke();
			_headerLabel.text = header;
			char[] markupChars = { '{', '}' };

			// Split body text into separate text parts and images
			string[] groups = Regex.Split(body, "({.+?})", RegexOptions.Multiline).Where(s => s != string.Empty).ToArray();

			Transform contentParent = _bodyLabel.transform.parent;
			// some 'magic' numbers
			int maxColumnCount = 3;
			Vector2 spacing = new Vector2(20, 5);
			foreach (string group in groups)
			{
				// Define parents, etc...
				Transform groupParent = contentParent;
				// split up for images and text
				string[] groupParts = Regex.Split(group, @"(!\([\w= ]+\))", RegexOptions.IgnorePatternWhitespace).ToArray();
				groupParts = groupParts.Where((part) => !string.IsNullOrEmpty(part.Trim(markupChars))).ToArray();

				int amountOfIconsPerRow = Math.Min(groupParts.Length, maxColumnCount);

				float largestAspectRatio = .5f;
				bool userAspectRatio = false;
				float defaultWidth = 0;

				GridLayoutGroup layoutGroup = null;
				// Create a new parent if needed
				if (group.StartsWith("{"))
				{
					// Set up the parent layout element
					GameObject imageParent = new GameObject("layout_grid");
					imageParent.transform.SetParent(contentParent, false);
					_temporaryObjects.Push(imageParent);
					// Set up layouting
					layoutGroup = imageParent.AddComponent<GridLayoutGroup>();

					defaultWidth = groupParent.GetComponent<RectTransform>().rect.width / amountOfIconsPerRow;
					defaultWidth -= spacing.x * amountOfIconsPerRow;
					layoutGroup.spacing = spacing;
					layoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
					layoutGroup.constraintCount = amountOfIconsPerRow;
					// replace group parent
					groupParent = imageParent.transform;

					Match matchAspectRatio = Regex.Match(group, " =[\\w.]*");
					if (matchAspectRatio.Success)
					{
						userAspectRatio = true;
						float.TryParse(matchAspectRatio.Value.Substring(2), out largestAspectRatio);
						string lastGroupPart = groupParts[groupParts.Length - 1];
						groupParts[groupParts.Length - 1] = lastGroupPart.Substring(0, lastGroupPart.Length - matchAspectRatio.Value.Length);
					}

				}
				// Generate group content
				for (int i = 0; i < groupParts.Length; i++)
				{
					// Images
					if (groupParts[i].StartsWith("!("))
					{
						// Remove image prefix and suffix
						string path = groupParts[i].Substring(2, groupParts[i].Length - 3);

						// Check if the image has a specific resolution set
						Match match = Regex.Match(path, " =\\w*x\\w*");
						if (match.Success)
						{
							path = path.Substring(0, path.Length - match.Length);
						}

						// Load the sprite from resources and add it as an image to the body
						Sprite sprite = Resources.Load<Sprite>("Images/" + path);
						Assert.IsNotNull(sprite, $"Sprite at {path} was not found ");

						if (!userAspectRatio)
						{
							float aspectRatio = sprite.rect.height / sprite.rect.width;
							if (largestAspectRatio < aspectRatio) largestAspectRatio = aspectRatio;
						}

						GameObject go = new GameObject("Image_" + path);
						_temporaryObjects.Push(go);
						go.transform.SetParent(groupParent.transform, false);
						Image image = go.AddComponent<Image>();
						image.preserveAspect = true;
						image.sprite = sprite;

						if (match.Success)
						{
							LayoutElement layout = go.AddComponent<LayoutElement>();
							string[] dimensions = match.Value.Substring(2).Split('x');
							int.TryParse(dimensions[0], out int width);
							int.TryParse(dimensions[1], out int height);
							if (width > 0) layout.preferredWidth = width;
							if (height > 0) layout.preferredHeight = height;
						}
					}
					else // Text
					{
						TextMeshProUGUI bodyPart = Instantiate(_bodyLabel.gameObject, groupParent).GetComponent<TextMeshProUGUI>();
						bodyPart.gameObject.SetActive(true);
						_temporaryObjects.Push(bodyPart.gameObject);
						bodyPart.text = groupParts[i];
					}
				}
				if (layoutGroup != null)
				{
					layoutGroup.cellSize = new Vector2(defaultWidth, defaultWidth * largestAspectRatio);
				}
			}

			// Finalize panel
			_closeButton.gameObject.SetActive(showCloseBtn);
		}

		// Add another UI prefab to the end of the info panel
		public InfoPanelFooter PushExtraInfoPrefab(InfoPanelFooter extraPrefab)
		{
			InfoPanelFooter footer = Instantiate(extraPrefab, _bodyLabel.transform.parent);
			footer.ParentPanel = this;
			_temporaryObjects.Push(footer.gameObject);
			return footer;
		}

		public void Close()
		{
			Reset();
			OnClose?.Invoke();
			this.gameObject.SetActive(false);
		}

		public void Reset()
		{
			while (_temporaryObjects.Count > 0) Destroy(_temporaryObjects.Pop());
			_infoHash = 0;
			_verticalScrollBar.value = 1;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (_tapToClose) Close();
		}
	}
}
