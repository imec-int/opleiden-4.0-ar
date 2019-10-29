using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Image))]
	public class SphereButton : MonoBehaviour
	{
		public Color Test;
		private Image image;

		private void Awake()
		{
			image = GetComponent<Image>();
		}

		private void Update()
		{
			//Test = image.canvasRenderer.GetColor();
			image.canvasRenderer.SetColor(Color.magenta);
			Test = image.canvasRenderer.GetColor();
			//image.material.SetColor("_Color",Color.magenta);
		}
	}
}