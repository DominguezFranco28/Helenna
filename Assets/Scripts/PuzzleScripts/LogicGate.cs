using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGate : MonoBehaviour
{
    [Header("Inputs")]
    public bool inputA = false;
    public bool inputB = false;

    [Header("Gate Type")]
    public GateType gateType = GateType.AND;

    public enum GateType
    {
        NOT,
        OR,
        XOR,
        AND,
        NOR,
        XNOR,
        NAND
    }

    // Evaluate the output based on selected gate type
    public bool CheckCondition()
    {
        switch (gateType)
        {
            case GateType.NOT:
                // Only consider inputA for NOT
                return !inputA;

            case GateType.OR:
                return inputA || inputB;

            case GateType.XOR:
                return inputA ^ inputB;

            case GateType.AND:
                return inputA && inputB;

            case GateType.NOR:
                return !(inputA || inputB);

            case GateType.XNOR:
                return !(inputA ^ inputB);

            case GateType.NAND:
                return !(inputA && inputB);

            default:
                return false;
        }
    }
}
