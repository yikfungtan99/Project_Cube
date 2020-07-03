using System;
using System.Threading;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    internal PuzzleModule _puzzleModule;
    public int componentId;

    public bool disabled = false;

    public event EventHandler<OnInteractedEventArgs> OnInteracted;
    
    public virtual void Start()
    {
        _puzzleModule = transform.GetComponentInParent<PuzzleModule>();
        OnInteracted += _puzzleModule.transform.parent.GetComponent<PlayerCube>().Action;
    }

    /// <summary>
    /// This is called whenever the button is clicked
    /// For networking function inherit base version
    /// base.Interact();
    /// This will trigger a reaction on the Reactor that have the moduleId and componentId of this Interactor
    /// </summary>
    public virtual void Interact()
    {
        if (disabled) return;
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
        public int DataInt;
    }
}