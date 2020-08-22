using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//This is the puzzle module game component
public class PuzzleModule:MonoBehaviour
{
    public int PuzzleId;
    public PuzzleManager puzzleManager;

    public PuzzleTypes puzzleType;
    public int puzzleVariation;
    public int puzzleRole;

    public bool moduleInitialized = false;
    public bool moduleCompleted = false;
    
    public List<Interactor> interactors = new List<Interactor>();
    public List<Reactor> reactors = new List<Reactor>();
    public List<Indicator> indicators = new List<Indicator>();

    public Indicator selfIndicator;
    public Indicator otherIndicator;
    public Indicator completeIndicator;

    private PuzzleMasterStorage puzzleStorage;

    [FormerlySerializedAs("_playerCube")] public PlayerCube playerCube;
    
    
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
            switch (indicator.indicatorType)
            {
                case IndicatorTypes.CompleteIndicator:
                    completeIndicator = indicator;
                    break;
                case IndicatorTypes.SelfIndicator:
                    selfIndicator = indicator;
                    break;
                case IndicatorTypes.OtherIndicator:
                    otherIndicator = indicator;
                    break;
            }
        }
        
        //Assign Puzzle Manager
        puzzleManager = spawnedPuzzle.GetComponent<PuzzleManager>();
        if (puzzleManager)
        {
            puzzleManager._puzzleModule = this;
            moduleInitialized = true;
            playerCube.CheckAllModuleInitialized();
            //puzzleManager.PuzzleStart();
        }
        else
        {
            print(gameObject.name + " puzzleManager not found");
        }
    }

    public void ToggleOtherIndicator()
    {
        if (!otherIndicator) return;
        if (!otherIndicator.isActive)
        {
            otherIndicator.ActiveIndicator();
        }
        else
        {
            otherIndicator.DeActivateIndicator();
        }
    } 
    
    public void ToggleSelfIndicator()
    {
        if (!selfIndicator) return;
        if (!selfIndicator.isActive)
        {
            selfIndicator.ActiveIndicator();
        }
        else
        {
            selfIndicator.DeActivateIndicator();
        }
    }

    public void ModuleComplete()
    {
        playerCube.CompletedModule(PuzzleId);
        SetModuleCompleted();
    }

    public void SetModuleCompleted()
    {
        moduleCompleted = true;
        DisableAllInteractors();
        otherIndicator.DeActivateIndicator();
        selfIndicator.DeActivateIndicator();
        LightUpCompleteIndicator();
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

    private void LightUpCompleteIndicator()
    {
        if (completeIndicator)
        {
            completeIndicator.ActiveIndicator();
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