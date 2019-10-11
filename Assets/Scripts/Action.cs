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
	public uint ID;
	public Part Part;
	public Handling Handling;
}
