using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlotsObject", menuName = "PigpenPuzzle/SlotsObject")]
public class SlotsObject : BoxData
{
    public void Awake()
    {
        boxType = BoxType.DropSlots;
    }
}
