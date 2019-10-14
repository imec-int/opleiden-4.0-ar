using System;

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

public class ActionData
{
	private int _Index;
	private Part _Part;
	private Operation _Operation;

	public int Index { get => _Index; set => _Index = value; }
	public Operation Operation { get => _Operation; set => _Operation = value; }
	public Part Part { get => _Part; set => _Part = value; }
}
