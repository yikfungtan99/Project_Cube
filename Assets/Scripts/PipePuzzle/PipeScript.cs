using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
    public PipePuzzleManager ppm;
    public int[] values;
    public float speed = 0.3f;
    float realRotation;

    //-START AND UPDATE------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void Start()
    {
        ppm = GameObject.FindGameObjectWithTag("PipePuzzleManager").GetComponent<PipePuzzleManager>();
    }

    void Update()
    {
        //FixRotation();    
    }

    private void OnMouseDown()
    {
        /*
        if(isFlipped)
        {
            Flip()
        }*/
        Rotate90Degrees();
        ppm.SetCurrentValue();
    }

    //-FUNCTIONS------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void Rotate90Degrees()
    {
        realRotation += 90;
        if(realRotation == 360)
        {
            realRotation = 0;
        }
        transform.rotation = Quaternion.Euler(0, 0, realRotation);

        RotateValues();
    }

    public void Rotate180Degrees() // ****NOT IN USE****
    {
        Rotate90Degrees();
        Rotate90Degrees();
    }

    public void Flip() // mirror the piece? seems unneccesary, use rotate 180 degrees instead ba // ****NOT IN USE****
    {

    }

    private void RotateValues() //change connect values
    {
        int aux = values[0];

        for (int i = 0; i < values.Length - 1; i++)
        {
            values [i] = values[i + 1];
        }
        values[3] = aux;
    }

    private void FixRotation() //speed doesn't work :( // ****NOT IN USE****
    {
        if (transform.root.eulerAngles.z != realRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, realRotation), speed);
        }
    }
}
