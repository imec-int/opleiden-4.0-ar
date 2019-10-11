public enum Handling
{
	None,
	Open,
	Close,
	Check,
	Start,
	Stop
}

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
	public uint Index;
	public Part Part;
	public Handling Handling;
}
