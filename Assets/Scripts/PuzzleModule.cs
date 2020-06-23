using System;
using UnityEngine;

//This is the puzzle module game component
public class PuzzleModule:MonoBehaviour
{
    public int PuzzleId;

    public PuzzleTypes puzzleType;
    public int puzzleVariation;
    public int puzzleRole;

    //This is to init the puzzle using PuzzleModuleData to spawn the correct puzzle
    public void SpawnPuzzle(int pt, int pv, int pr)
    {
        puzzleType = (PuzzleTypes) pt;
        puzzleVariation = pv;
        puzzleRole = pr;
        
        print("Cube" + PuzzleId 
                  + " spawned Cube according to data \n"
                  + "PuzzleType: " + pt + "\n"
                  + "PuzzleVariation: " + pv  + "\n"
                  + "PuzzleRole: " + pr + "\n");
    }
}    