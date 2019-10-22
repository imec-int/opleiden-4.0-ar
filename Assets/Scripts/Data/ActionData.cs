using System;
using UnityEngine;

[Serializable]
public enum Operation
{
	None,
	Open,
	Close,
	Check,
	Start,
	Stop
}

[Serializable]
public enum Part
{
	None,
	Pump,
	Valve,
	OilLevel,
	Barometer
}

// Base class describing an action
[Serializable]
public class ActionData
{
	[SerializeField]
	private Part _Part;
	[SerializeField]
	private Operation _Operation;

	public Operation Operation { get => _Operation; set => _Operation = value; }
	public Part Part { get => _Part; set => _Part = value; }

	// because two different enum combinations can give the same numbers, we need to offset one
	private const int magicoffset= 100;
	public int UID
	{
		get
		{
			return (_Operation.GetHashCode()*magicoffset) + _Part.GetHashCode();
		}
	}
}

// Class to be used for anything related to actions on the timeline
[Serializable]
public class IndexedActionData: ActionData
{
	private int _Index;
	public int Index { get => _Index; set => _Index = value; }
}