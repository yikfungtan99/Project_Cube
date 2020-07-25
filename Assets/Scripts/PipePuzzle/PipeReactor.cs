using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Numerics;
using UnityEngine;

//Should inherit from Reactor
public class PipeReactor : Reactor
{
    public PipePuzzleManager ppm;
    public int[] values;
    public bool inversed;
    float realRotation;

    private UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.identity;
    private UnityEngine.Quaternion currentRotation;
    //private Transform currentTransform;
    float rotationSpeed = 500;

    private void Start()
    {
        ppm = GetComponentInParent<PuzzleModule>().puzzleManager as PipePuzzleManager;
        currentRotation = transform.localRotation;
    }

    private void Update()
    {
        transform.localRotation = UnityEngine.Quaternion.RotateTowards(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
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
        targetRotation = UnityEngine.Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z + 90);
        currentRotation = UnityEngine.Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z + 90);
    }

    private void InverseRotatePipe()
    {
        targetRotation = UnityEngine.Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z - 90);
        currentRotation = UnityEngine.Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z - 90);
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
