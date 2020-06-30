using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Should inherit from interactor
public class PipeButton: Interactor
{
    private PipePuzzleManager ppm;

    public override void Start()
    {
        base.Start();
        ppm = _puzzleModule.puzzleManager as PipePuzzleManager;
    }

    public override void Interact()
    {
        print("Pipe Button Clicked");
        base.Interact();
        ppm.Sweep();
    }
    
    //Interaction here
    // private void OnMouseDown()
    // {
    //     ppm.ButtonPress((int)transform.localPosition.x,(int)transform.localPosition.y);
    // }
    
    // public override void Interact()
    // {
    //     print("Clicked");
    //     base.Interact();
    //     AnimPressButton();
    // }

    //-FUNCTIONS------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


}
