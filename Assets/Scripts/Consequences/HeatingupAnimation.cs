using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Consequences
{
	[RequireComponent(typeof(Light))]
	public class HeatingupAnimation : MonoBehaviour
	{
		private Light _attachedLight;
		private float _maxIntensity;
		private float _minIntensity;

		[SerializeField]
		private float _duration = 2;

		private float _startTime = 0;

		// Start is called before the first frame update
		void Start()
		{
			_attachedLight = this.GetComponent<Light>();
			_maxIntensity = _attachedLight.intensity;
			_minIntensity = _attachedLight.intensity * 0.15f;
			_startTime = Time.time;
			_attachedLight.intensity = 0;
		}

		// Update is called once per frame
		void Update()
		{
			float progress = Mathf.Clamp01((Time.time - _startTime) / _duration);
			_attachedLight.intensity = ((_maxIntensity-_minIntensity) * Mathf.Clamp01(sinwave(0.5f, progress))) + (_minIntensity * progress);
		}

		private float sinwave(float speed = 0.5f, float offset = 0)
		{
			return (Mathf.Sin(Time.time - _startTime)*speed) + offset;
		}
	}
}