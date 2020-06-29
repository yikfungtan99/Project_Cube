using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Should inherit from Reactor
public class PipeReactor : Reactor
{
    public PipePuzzleManager ppm;
    public int[] values;
    public float speed = 0.3f;
    float realRotation;

    private void Start()
    {
        ppm = GetComponentInParent<PuzzleModule>().puzzleManager as PipePuzzleManager;
    }

    //Reaction here
    public void Rotate90Degrees()
    {
        realRotation += 90;
        if(realRotation == 360)
        {
            realRotation = 0;
        }
        transform.localRotation = Quaternion.Euler(0, 0, realRotation);

        //RotateValues();
    }
    
    public override void ReAct()
    {
        //Rotate90Degrees();
        RotatePipe();
    }

    private void RotatePipe()
    {
        print("Rotate");
        transform.RotateAround(transform.position, transform.forward, 90);
    }

    private void RotateValues() //change connect values
    {
        // int aux = values[0];
        //
        // for (int i = 0; i < values.Length - 1; i++)
        // {
        //     values [i] = values[i + 1];
        // }
        // values[3] = aux;
        
        transform.Rotate(new Vector3(0, 0, 90));
    }

    // private void FixRotation() //speed doesn't work :( // ****NOT IN USE****
    // {
    //     if (transform.root.eulerAngles.z != realRotation)
    //     {
    //         transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, realRotation), speed);
    //     }
    // }
}
