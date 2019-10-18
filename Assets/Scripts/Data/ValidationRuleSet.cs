using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ValidationRuleSet", menuName = "opleiden-4.0-ar/ValidationRuleSet", order = 1)]
public class ValidationRuleSet: ScriptableObject
{
    [SerializeField]
    private List<ActionData> _ActionsInOrder;

    [SerializeField]
    private int _PositionTolerance = 2;

    [SerializeField]
    private bool _CheckDisplacement = true;

    public List<ActionData> ActionsInOrderList
    {
        get => _ActionsInOrder;
    }
    public int PositionTolerance
    {
        get => _PositionTolerance;
    }

    // Checks whether the ruleset can be achieved
    public bool CheckIfValid()
    {
        foreach(ActionData actionData in _ActionsInOrder)
        {
            if(actionData.Operation == Operation.None || actionData.Part == Part.None)
            {
                return false;
            }
        }
        return true;
    }
    public void Setup()
    {
        // Ensure parity with the ActionController index
        for (int i = 0; i < _ActionsInOrder.Count; ++i)
        {
            _ActionsInOrder[i].Index = i + 1;
        }
    }
}