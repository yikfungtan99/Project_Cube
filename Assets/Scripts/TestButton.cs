using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour, IInteractable
{
    private PuzzleModule _puzzleModule;
    public event EventHandler<OnInteractedEventArgs> OnInteracted;
    private Animator _anim;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _puzzleModule = GetComponentInParent<PuzzleModule>();
    }
    
    public void Interact()
    {
        Debug.Log("Click");
        AnimPressButton();
        OnInteracted?.Invoke(this, new OnInteractedEventArgs{Id = _puzzleModule.PuzzleId});
    }

    private void AnimPressButton()
    {
        _anim.SetTrigger("isPressed");
    }
}
