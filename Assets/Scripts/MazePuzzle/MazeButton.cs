using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeButton : Interactor
{
    //private Animator anim;
    public int direction;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //anim = GetComponent<Animator>();
    }

    public override void Interact()
    {
        //anim.SetTrigger("isPressed");
        //print("Pipe Button Clicked");
       _puzzleModule.playerCube.MazePuzzleButton(_puzzleModule.PuzzleId, direction);
        //mpm.MazeButtonPress(direction);
    }

    public void SetDirection(int i)
    {
        direction = i; // 0 = up, 1 = right, 2 = down, 3 = left
    }
}
