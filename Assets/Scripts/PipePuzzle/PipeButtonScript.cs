using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeButtonScript : MonoBehaviour
{
    public PipePuzzleManager ppm;
    //-START AND UPDATE------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        ppm = GameObject.FindGameObjectWithTag("PipePuzzleManager").GetComponent<PipePuzzleManager>();
    }
    
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        ppm.ButtonPress((int)transform.localPosition.x,(int)transform.localPosition.y);
    }

    //-FUNCTIONS------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


}
