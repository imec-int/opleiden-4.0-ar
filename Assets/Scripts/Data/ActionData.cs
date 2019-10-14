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
	private uint index;
	private Part part;
	private Operation operation;

	public uint Index { get => index; set => index = value; }
	public Operation Operation { get => operation; set => operation = value; }
	public Part Part { get => part; set => part = value; }

	//public event System.Action<bool> MyAction;


	public delegate void ActionEvent(ActionData action);

	public ActionEvent Delete;
	public ActionEvent Update;

}
