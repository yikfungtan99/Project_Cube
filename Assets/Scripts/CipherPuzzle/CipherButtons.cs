using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class CipherButtons : Interactor
{
    public CipherPuzzleManager cpm;
    private Animator anim;
    public TextMeshPro letterEncoded;
    public string letter;
    private string _letterCode;

    private void Awake()
    {
        letterEncoded = GetComponentInChildren<TextMeshPro>();
        anim = GetComponentInChildren<Animator>();
    }

    //-START AND UPDATE------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        //cpm = GetComponentInParent<CipherPuzzleManager>();
        letterEncoded = GetComponentInChildren<TextMeshPro>();
    }

    //-FUNCTIONS------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public string GetLetter()
    {
        return letter;
    }
    
    public void SetLetter(string s)
    {
        letter = s;
        letter = Regex.Replace(s, "[^a-zA-Z]", "");

        if (cpm)
        {
            if (letterEncoded)
            {
                _letterCode = cpm.Encode(letter).ToUpper();
                letterEncoded.SetText(_letterCode);
            }
            else
            {
                print("letterEncoded Not Found");
            }
        }
        else
        {
            print("Cpm not found");
        }
        
        //str = cpm.Encode(letter);
        //str = str.ToUpper();
        //letterEncoded.SetText(str);

    }

    //Testing
    private void OnMouseDown()
    {
        //cpm.KeyboardButtonPress(letterEncoded.text);
    }
    
    //Actual
    public override void Interact()
    {
        if (disabled) return;
        print("Clicked");
        anim.SetTrigger("isPressed");
        cpm.KeyboardButtonPress(_letterCode);
    }

}
