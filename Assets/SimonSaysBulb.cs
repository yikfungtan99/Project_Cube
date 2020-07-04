using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysBulb : Reactor
{
    [SerializeField] private GameObject bulb;

    public override void ReAct()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        LightUp();
        yield return new WaitForSeconds(0.5f);
        LightOut();
    }

    public void LightUp()
    {
        bulb.SetActive(true);
    }
    
    public void LightOut()
    {
        bulb.SetActive(false);
    }
}
