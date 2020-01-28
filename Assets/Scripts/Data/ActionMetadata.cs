using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Data
{
	[Serializable] public class PartInfoDictionary : SerializableDictionaryBase<PartType, ActionPartInfo> { }
	[Serializable] public class OperationInfoDictionary : SerializableDictionaryBase<Operation, ActionOperationInfo> { }

	[CreateAssetMenu(fileName = "ActionMetadata", menuName = "opleiden-4.0-ar/ActionMetadata", order = 0)]
	public class ActionMetadata : ScriptableObject
	{
		[SerializeField]
		private PartInfoDictionary _ActionPartsInfo = new PartInfoDictionary();

		[SerializeField]
		private OperationInfoDictionary _ActionOperationsInfo = new OperationInfoDictionary();

		public PartInfoDictionary ActionPartsInfo { get => _ActionPartsInfo; }
		public OperationInfoDictionary ActionOperationsInfo { get => _ActionOperationsInfo; }
	}

	[Serializable]
	public struct ActionPartInfo
	{
		public string Name, Icon;
	}

	[Serializable]
	public struct ActionOperationInfo
	{
		public string Name;
	}
}
