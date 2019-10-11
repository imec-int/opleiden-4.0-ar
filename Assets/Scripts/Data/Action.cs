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

public class Action
{
	private uint index;
	private Part part;
	private Operation operation;

	public delegate void ActionEvent(Action action);

	public ActionEvent Delete;
	public ActionEvent Update;

	public uint Index { get => index; set => index = value; }
	public Operation Operation { get => operation; set => operation = value; }
	public Part Part { get => part; set => part = value; }
}
