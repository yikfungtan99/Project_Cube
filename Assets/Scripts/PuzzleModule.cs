using System;
using UnityEngine;

//This is the puzzle module game component
public class PuzzleModule:MonoBehaviour
{
    public int PuzzleId;
    public PuzzleModuleData moduleData;

    //This is to init the puzzle using PuzzleModuleData to spawn the correct puzzle
    public void SpawnPuzzle()
    {
        print("Cube" + PuzzleId 
                  + " spawned Cube according to data \n"
                  + "PuzzleType: " + moduleData.PuzzleType + "\n"
                  + "PuzzleVariation: " +moduleData.PuzzleVariation  + "\n"
                  + "PuzzleRole: " +moduleData.PuzzleRole+ "\n"); 
    }
}