using System;
using System.Threading;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private PuzzleModule _puzzleModule;
    public int componentId;

    public event EventHandler<OnInteractedEventArgs> OnInteracted;
    
    public virtual void Start()
    {
        _puzzleModule = transform.GetComponentInParent<PuzzleModule>();
        OnInteracted += _puzzleModule.transform.parent.GetComponent<PlayerCube>().Action;
    }

    public virtual void Interact()
    {
        if (_puzzleModule)
        {
            print("Interact");    
            OnInteracted?.Invoke(this, new OnInteractedEventArgs {ModuleId = _puzzleModule.PuzzleId, ComponentId = componentId});
        }
        else
        {
            Debug.LogError("Puzzle Module Not Found!");
        }
    }

    public class OnInteractedEventArgs : EventArgs
    {
        public int ModuleId;
        public int ComponentId;
    }
}