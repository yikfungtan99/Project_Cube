using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TestBulb : PuzzleModule, IReactable
{
    [SerializeField] private Light bulbLight;

    private void LightUp()
    {
        Debug.Log("Light Up");
        bulbLight.gameObject.SetActive(true);
    }

    public void ReAct()
    {
        Debug.Log("REACTED");
        LightUp();
    }
}
