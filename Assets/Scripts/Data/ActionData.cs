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
	private int index;
	private Part part;
	private Operation operation;

	public int Index { get => index; set => index = value; }
	public Operation Operation { get => operation; set => operation = value; }
	public Part Part { get => part; set => part = value; }
}
