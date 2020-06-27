using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TestBulb : Reactor
{
    [SerializeField] private GameObject bulbLight;

    private void Awake()
    {
        bulbLight.SetActive(false);
    }

    private void LightUp()
    {
        print(bulbLight.name + "Lit");
        bulbLight.SetActive(true);
    }

    public override void ReAct()
    {
        Debug.Log("REACTED");
        LightUp();
    }
}