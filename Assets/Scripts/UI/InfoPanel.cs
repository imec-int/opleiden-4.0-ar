using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Data;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace UI
{
	public class InfoPanel : MonoBehaviour
	{
		public event Action OnOpen;
		public event Action OnClose;

		[SerializeField]
		private TextMeshProUGUI _headerLabel;

		[SerializeField]
		private TextMeshProUGUI _bodyLabel;

		[SerializeField]
		private Button _closeButton;

		private Stack<GameObject> _temporaryObjects = new Stack<GameObject>();

		public void Awake()
		{
			_closeButton.onClick.AddListener(Close);
		}

		public void Show(HighlightInfo info)
		{
			Show(info.Header, info.Body);
		}

		public void Show(string header, string body)
		{
			OnOpen?.Invoke();
			_headerLabel.text = header;

			// Split body text into separate text parts and images
			string[] bodyParts = Regex.Split(body, @"(!\(\w+\))", RegexOptions.IgnorePatternWhitespace).Where(s => s != string.Empty).ToArray();

			bool firstTextBlock = true;
			Transform bodyParent = _bodyLabel.transform.parent;

			for (int i = 0; i < bodyParts.Length; i++)
			{
				if (bodyParts[i].StartsWith("!("))
				{
					// Remove image prefix and suffix
					string path = bodyParts[i].Substring(2, bodyParts[i].Length - 3);

					// Load the sprite from resources and add it as an image to the body
					Sprite sprite = Resources.Load<Sprite>("Images/" + path);
					Assert.IsNotNull(sprite, $"Sprite at {path} was not found ");
					GameObject go = new GameObject("Image_" + path);
					_temporaryObjects.Push(go);
					go.transform.SetParent(bodyParent, false);
					Image image = go.AddComponent<Image>();
					image.preserveAspect = true;
					image.sprite = sprite;
				}
				else
				{
					// When it is the first text block in the body use the default existing _BodyLabel or else instantiate it for additional text blocks
					if (firstTextBlock)
					{
						_bodyLabel.text = bodyParts[i];
						_bodyLabel.transform.SetAsLastSibling();
						firstTextBlock = false;
					}
					else
					{
						TextMeshProUGUI bodyPart = Instantiate(_bodyLabel.gameObject, bodyParent).GetComponent<TextMeshProUGUI>();
						_temporaryObjects.Push(bodyPart.gameObject);
						bodyPart.text = bodyParts[i];
					}
				}
			}

			this.gameObject.SetActive(true);
		}

		// Add another UI prefab to the end of the info panel
		public void PushExtraObject(GameObject extraPrefab)
		{
			GameObject go = Instantiate(extraPrefab, _bodyLabel.transform.parent);
			_temporaryObjects.Append(go);
		}

		public void Close()
		{
			for (int i = 0; i < _temporaryObjects.Count; i++)
			{
				Destroy(_temporaryObjects.Pop());
			}

			OnClose?.Invoke();
			this.gameObject.SetActive(false);
		}
	}
}
