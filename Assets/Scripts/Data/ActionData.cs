using System;
using UnityEngine;
using TimeLineValidation;

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

[Serializable()]
public class ActionData
{
	[SerializeField]
	private Part _Part;
	[SerializeField]
	private Operation _Operation;

	public Operation Operation { get => _Operation; set => _Operation = value; }
	public Part Part { get => _Part; set => _Part = value; }

	// because a low operation + high part can give same numbers as high part + low operation, we need to offset one
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
[Serializable()]
public class IndexedActionData: ActionData
{
	private int _Index;
	public int Index { get => _Index; set => _Index = value; }

/*
	public ValidationResult ValidateAgainst(ActionData other)
	{
		if (other == null)
			return ValidationResult.Unnecessary;

		if(this == other)
			return ValidationResult.Correct;

		if(Part == other.Part && Operation == other.Operation)
			return ValidationResult.IncorrectIndex;
		if (Part == other.Part && Operation != other.Operation)
			return ValidationResult.IncorrectOperation;
		if (Part != other.Part && Operation == other.Operation)
			return ValidationResult.IncorrectPart;
					
		return ValidationResult.CompletelyIncorrect;
	}

#region Equality implementation
    public bool Equals(ActionData other)
    {
        if (other == null)
			return false;

		return (Index == other.Index 
		&& Operation == other.Operation
		&& Part == other.Part);
    }

	public override int GetHashCode()
	{
		return Part.GetHashCode() ^ Operation.GetHashCode() ^ Index.GetHashCode();
	}

	public static bool operator == (ActionData data1, ActionData data2)
	{
		if ((object)data1 == null)
            return (object)data2 == null;
		return data1.Equals(data2);
	}

	public static bool operator != (ActionData data1, ActionData data2)
	{
		return !(data1==data2);
	}
#endregion
*/
}