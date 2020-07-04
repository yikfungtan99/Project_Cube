using System;
using System.Collections.Generic;
using UnityEngine;

//This is the puzzle module game component
public class PuzzleModule:MonoBehaviour
{
    public int PuzzleId;
    public PuzzleManager puzzleManager;

    public PuzzleTypes puzzleType;
    public int puzzleVariation;
    public int puzzleRole;

    public bool moduleCompleted = false;
    
    public List<Interactor> interactors = new List<Interactor>();
    public List<Reactor> reactors = new List<Reactor>();
    public List<Indicator> indicators = new List<Indicator>();

    private PuzzleMasterStorage puzzleStorage;

    [SerializeField] private PlayerCube _playerCube;

    void Start()
    {
        puzzleManager = GetComponentInChildren<PuzzleManager>();    
    }

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

        GameObject spawnedPuzzle = Instantiate(puzzleToSpawn, transform.position, transform.rotation);
        spawnedPuzzle.transform.parent = transform;

        foreach (var interactor in spawnedPuzzle.GetComponentsInChildren<Interactor>())
        {
            interactor.componentId = interactors.Count;
            interactors.Add(interactor);
        }

        foreach (var reactor in spawnedPuzzle.GetComponentsInChildren<Reactor>())
        {
            reactors.Add(reactor);
        }

        foreach (var indicator in spawnedPuzzle.GetComponentsInChildren<Indicator>())
        {
            indicators.Add(indicator);
        }
        
        //Assign Puzzle Manager
        puzzleManager = GetComponentInChildren<PuzzleManager>();
    }

    public void ModuleComplete()
    {
        _playerCube.CompletedModule(PuzzleId);
        SetModuleCompleted();
    }

    public void SetModuleCompleted()
    {
        moduleCompleted = true;
        DisableAllInteractors();
        EnableIndicators();
    }

    private void DisableAllInteractors()
    {
        if (interactors.Count > 0)
        {
            print("disabling all interactors");
            foreach (var interactor in interactors)
            {
                interactor.disabled = true;
            }
        }
    }

    private void EnableIndicators()
    {
        if (indicators.Count > 0)
        {
            foreach (var indicator in indicators)
            {
                indicator.ActivateIndicator();
            }
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