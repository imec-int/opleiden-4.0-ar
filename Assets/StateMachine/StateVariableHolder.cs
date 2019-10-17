using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateVariableHolder : MonoBehaviour
{
	[SerializeField]
	private GameObject _LoadingScreen;
	[SerializeField]
	private GameObject _TimeLine;
	[SerializeField]
	private GameObject _PumpTagField;

	[SerializeField]
	private UIHighlightContainer _HighlightContainer;

	public GameObject LoadingScreen
	{
		get
		{
			return _LoadingScreen;
		}
	}

	public GameObject TimeLine
	{
		get
		{
			return _TimeLine;
		}
	}

	public UIHighlightContainer HighlightContainer
	{
		get
		{
			return _HighlightContainer;
		}
	}

	public GameObject PumpTagField => _PumpTagField;
}
