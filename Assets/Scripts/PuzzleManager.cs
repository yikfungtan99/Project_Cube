using System;
using UnityEngine;

/// <summary>
/// Inherit from this class to extend puzzle logic such as win conditions
/// Think of this class as an extension to the puzzleModule but handle only logic
/// This is only used when the interactor and reactor script is not enough to handle the puzzle logic.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    public PuzzleModule _puzzleModule;

    public virtual void Start()
    {
        _puzzleModule = GetComponentInParent<PuzzleModule>();
    }
}
