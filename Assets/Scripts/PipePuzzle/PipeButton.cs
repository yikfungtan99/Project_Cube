using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Should inherit from interactor
public class PipeButton: Interactor
{
    private PipePuzzleManager ppm;
    private Animator anim;

    public override void Start()
    {
        base.Start();
        ppm = _puzzleModule.puzzleManager as PipePuzzleManager;
        anim = GetComponent<Animator>();
    }

    public override void Interact()
    {
        anim.SetTrigger("isPressed");
        print("Pipe Button Clicked");
        base.Interact();
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
