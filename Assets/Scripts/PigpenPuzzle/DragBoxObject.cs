using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DragBoxObject", menuName = "PigpenPuzzle/DragBox")]
public class DragBoxObject : BoxData
{
    public void Awake()
    {
        boxType = BoxType.DraggableBox;
    }
}
