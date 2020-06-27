using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestButton : Interactor
{
    private Animator _anim;
    public override void Start()
    {
        base.Start();
        _anim = GetComponent<Animator>();
    }

    public override void Interact()
    {
        print("Clicked");
        base.Interact();
        AnimPressButton();
    }

    private void AnimPressButton()
    {
        _anim.SetTrigger("isPressed");
    }
}
