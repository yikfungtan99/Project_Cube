﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CubeStates
{
    Rotate,
    Examine
}

public class CubeState : MonoBehaviour
{
    public CubeStates currentState;
    private bool autoCalibrateCube = true;
    
    public void ChangeState()
    {
        if ((int) currentState < Enum.GetValues(typeof(CubeStates)).Length - 1)
        {
            currentState += 1;
            if (currentState == CubeStates.Examine)
            {
                AutoCalibrate();
            }
        }
        else
        {
            currentState = (CubeStates) 0;
        }
    }

    public void AutoCalibrate()
    {
        if (!autoCalibrateCube) return;
        GetComponent<CubeCalibration>().CalibrateCube();
    }
}
