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
		CheckBlind,
		Start,
		Stop,
		TowardsPump,
		TowardsBlind,
		Label
	}

	[Serializable]
	public enum PartType
	{
		None,
		Pump,
		Valve,
		OilLevel,
		Barometer,
		ProtectionCap,
		Safety,
		Ventilation,
		ThreeWayValve
	}

	// Base class describing an action
	[Serializable]
	public class ActionData
	{
		[SerializeField]
		private GameObject _part;

		[SerializeField]
		private PartType _partType;

		[SerializeField]
		private Operation _operation;

		public Operation Operation { get => _operation; set => _operation = value; }
		public PartType PartType { get => _partType; set => _partType = value; }
		public GameObject Part { get => _part; set => _part = value; }

		public override int GetHashCode()
		{
			// because two different enum combinations can give the same numbers, we need to offset one
			return _operation.GetHashCode() + ((int)_partType + Enum.GetNames(typeof(Operation)).Length).GetHashCode() + _part.name.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return GetHashCode() == (obj as ActionData)?.GetHashCode();
		}

		public override string ToString()
		{
			return $"{_part.name}: {_operation} was performed on {_partType}";
		}
	}

	// Class to be used for anything related to actions on the timeline
	[Serializable]
	public class IndexedActionData : ActionData
	{
		private int _index;
		public int Index { get => _index; set => _index = value; }
	}
}
