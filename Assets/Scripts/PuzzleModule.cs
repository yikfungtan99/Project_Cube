﻿using System;
using System.Collections.Generic;
using UnityEngine;

//This is the puzzle module game component
public class PuzzleModule:MonoBehaviour
{
    public int PuzzleId;

    public PuzzleTypes puzzleType;
    public int puzzleVariation;
    public int puzzleRole;

    public bool moduleCompleted;
    
    public List<Interactor> interactors = new List<Interactor>();
    public List<Reactor> reactors = new List<Reactor>();

    public event EventHandler OnPuzzleModuleComplete;

    private PuzzleMasterStorage puzzleStorage;

    [SerializeField] private PlayerCube _playerCube;
    
    //This is to init the puzzle using PuzzleModuleData to spawn the correct puzzle
    public void SpawnPuzzle(int pt, int pv, int pr)
    {
        puzzleType = (PuzzleTypes) pt;
        puzzleVariation = pv;
        puzzleRole = pr;
        
        //SpawnLog(pt, pv, pr);

        puzzleStorage = PuzzleMasterStorage.Instance;

        GameObject puzzleToSpawn = (pr < 0)
            ? puzzleStorage.puzzleTypes[pt].puzzleVariation[pv].puzzleInteractor
            : puzzleStorage.puzzleTypes[pt].puzzleVariation[pv].puzzleReactor;

        GameObject spawnedPuzzle = Instantiate(puzzleToSpawn, transform);

        foreach (var interactor in spawnedPuzzle.GetComponentsInChildren<Interactor>())
        {
            interactor.componentId = interactors.Count;
            print(PuzzleId + ": Interactors " + interactors.Count);
            interactors.Add(interactor);
        }

        foreach (var reactor in spawnedPuzzle.GetComponentsInChildren<Reactor>())
        {
            reactors.Add(reactor);
            print(PuzzleId + "Reactors " + reactors.Count);
        }
    }

    private void SpawnLog(int pt, int pv, int pr)
    {
        print("Cube" + PuzzleId 
                     + " spawned Cube according to data \n"
                     + "PuzzleType: " + pt + "\n"
                     + "PuzzleVariation: " + pv  + "\n"
                     + "PuzzleRole: " + pr + "\n");
    }
}    