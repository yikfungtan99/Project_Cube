using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class KeyboardButtons : MonoBehaviour
{
    public CipherPuzzleManager cpm;
    public TextMeshPro letterEncoded;
    public string letter;
    //private string str;

    //-START AND UPDATE------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        cpm = GetComponentInParent<CipherPuzzleManager>();
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
        letterEncoded.SetText(cpm.Encode(letter).ToUpper());
        //str = cpm.Encode(letter);
        //str = str.ToUpper();
        //letterEncoded.SetText(str);

    }

    private void OnMouseDown()
    {
        cpm.KeyboardButtonPress(letterEncoded.text);
    }

    /* for yik fung 
    public override void Interact()
    {
        base.Interact();
    }
    */
}
