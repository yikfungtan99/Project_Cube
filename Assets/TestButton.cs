using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : PuzzleModule, IInteractable
{
    public event EventHandler<OnInteractedEventArgs> OnInteracted;
    private Animator _anim;
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    
    public void Interact()
    {
        AnimPressButton();
        OnInteracted?.Invoke(this, new OnInteractedEventArgs{Id = base.PuzzleId});
    }

    private void AnimPressButton()
    {
        _anim.SetTrigger("isPressed");
    }
}
