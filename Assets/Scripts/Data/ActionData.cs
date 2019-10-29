using System;
using UnityEngine;

namespace Data
{
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
		private const int magicoffset = 100;

		public static int CalculateUID(Operation operation, Part part)
		{
			return (operation.GetHashCode() * magicoffset) + part.GetHashCode();
		}

		public int UID
		{
			get
			{
				return CalculateUID(_Operation, _Part);
			}
		}
	}

	// Class to be used for anything related to actions on the timeline
	[Serializable]
	public class IndexedActionData : ActionData
	{
		private int _Index;
		public int Index { get => _Index; set => _Index = value; }
	}
}
