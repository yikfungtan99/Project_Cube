using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class SimonSaysButton : Interactor
{
    private SimonSaysPuzzleManager _simonSaysPuzzleManager;
    
    public EventHandler<OnSimonSaysButtonPressedButtonEventArgs> OnSimonSaysButtonPressed;

    [SerializeField] private Animator anim;

    [SerializeField] private float _buttonCooldown = 0.3f;
    private bool _canPress = true;
    
    public override void Start()
    {
        _puzzleModule = transform.GetComponentInParent<PuzzleModule>();
        _simonSaysPuzzleManager = _puzzleModule.puzzleManager as SimonSaysPuzzleManager;
        OnSimonSaysButtonPressed += _simonSaysPuzzleManager.PressedButton;
    }

    public override void Interact()
    {
        if (!_canPress) return;
        
        if (!_simonSaysPuzzleManager)
        {
            print("SimonSays PuzzleManager Not Found");
            return;
        }

        anim.SetTrigger("isPressed");
        StartCoroutine(Cooldown());
        
        OnSimonSaysButtonPressed?.Invoke(this, new OnSimonSaysButtonPressedButtonEventArgs() {ComponentId = componentId});
    }

    IEnumerator Cooldown()
    {
        _canPress = false;
        yield return new WaitForSeconds(_buttonCooldown);
        _canPress = true;
    }

    public class OnSimonSaysButtonPressedButtonEventArgs : EventArgs
    {
        public int ComponentId;
    }
}
