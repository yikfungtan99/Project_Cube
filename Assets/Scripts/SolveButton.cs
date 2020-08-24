using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveButton : MonoBehaviour
{
    private Animator anim;
    private PuzzleManager pm;
    public bool disabled = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        if(!disabled)
        {
            pm.CheckWin();
            anim.SetTrigger("isPressed");
        }
    }

    public void SetManager(PuzzleManager manager)
    {
        pm = manager;
    }
}
