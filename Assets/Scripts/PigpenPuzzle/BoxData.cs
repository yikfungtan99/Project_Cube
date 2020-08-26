using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxType
{
    DraggableBox,
    DropSlots
}

public enum BoxIndex
{
    empty = 0,
    firstAlphabet = 1,
    secondAlphabet = 2,
    thirdAlphabet = 3,
    fourthAlphabet = 4,
    fifthAlphabet = 5,
    sixthAlphabet = 6,
}

public class BoxData : ScriptableObject
{
    public GameObject boxPrefabs;
    public BoxType boxType;
    public BoxIndex boxIndex;
    [TextArea(1,2)] public string description;
}
