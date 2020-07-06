using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Should inherit from Reactor
public class PipeReactor : Reactor
{
    public PipePuzzleManager ppm;
    public int[] values;
    public bool inversed;
    float realRotation;

    private void Start()
    {
        ppm = GetComponentInParent<PuzzleModule>().puzzleManager as PipePuzzleManager;
    }
    
    public override void ReAct()
    {
        Rotate();
        ppm.pipePuzzle.currentValue = ppm.Sweep(); // check current connections
        ppm.CheckForWin();
    }

    public void Rotate()
    {
        if (inversed)
        {
            RotatePipe(); // rotate pipe transform
            RotateValues(); // change connection values when rotating
        }
        else
        {
            InverseRotatePipe(); // rotate pipe transform
            InverseRotateValues(); // change connection values when rotating
        }
    }

    private void RotatePipe()
    {
        transform.RotateAround(transform.position, transform.forward, 90);
    }

    private void InverseRotatePipe()
    {
        transform.RotateAround(transform.position, transform.forward, -90);
    }

    private void RotateValues() //change connect values
    {
        int aux = values[0];
        
        for (int i = 0; i < values.Length - 1; i++)
        {
            values [i] = values[i + 1];
        }
        values[3] = aux;
        
        //transform.Rotate(new Vector3(0, 0, 90));
    }

    private void InverseRotateValues() //change connect values
    {
        int aux = values[3];

        for (int i = values.Length - 1; i > 0 ; i--)
        {
            values[i] = values[i - 1];
        }
        values[0] = aux;
    }
    // private void FixRotation() //speed doesn't work :( // ****NOT IN USE****
    // {
    //     if (transform.root.eulerAngles.z != realRotation)
    //     {
    //         transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, realRotation), speed);
    //     }
    // }
}
