using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PigpenButton : Interactor
{
    //public static event Action ansCorrectPass;
    private PigpenManager _pigpenManager;

    private void Start()
    {
        _pigpenManager = GetComponentInParent<PigpenManager>();
    }
    
    private void OnMouseDown()
    {
        //?.Invoke();
    }

    public override void Interact()
    {
        print("Interact");

        if (_pigpenManager)
        {
            if (_pigpenManager._puzzleModule)
            {
                if (_pigpenManager._puzzleModule.playerCube)
                {
                    _pigpenManager._puzzleModule.playerCube.PigpenPuzzle( _pigpenManager._puzzleModule.PuzzleId);
                }
                else
                {
                    print("playerCube not found");
                }
            }
            else
            {
                print("puzzleModule not found");
            }
        }
        else
        {
            print("_pigpenManager not found");
        }
    }
}
