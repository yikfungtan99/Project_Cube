using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PigpenSolveButton : MonoBehaviour
{
    public static event Action ansCorrectPass;

    private void OnMouseDown()
    {
        ansCorrectPass?.Invoke();
    }
}
