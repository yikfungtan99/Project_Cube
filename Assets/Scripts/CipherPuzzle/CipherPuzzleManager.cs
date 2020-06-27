using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CipherPuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class CipherPuzzle
    {
        public int cipher; // store which cipher to use (1 = Atbash, 2 = CaeserForward, 3 = CaeserBackward)
        public int cipherHint; // store which cipher hint to use based on cipher chosen (if atbash, randomly select atbash hint preset (1 = AtbashHint1, 2 = AtbashHint2, etc)
        public int picture; // store which picture to use (1 = monkeyPicture, 2 = birdPicture, 3 = tigerPicture)
        public int answer; // store which answer is required to solve puzzle based on picture stored (if monkey, 1 = monkey, 2 = chimp)
        //public CipherSlotScript[,] buttons;
        //public CipherButtonScript[,] buttons;
    }
    public CipherPuzzle cipherPuzzle;

    //
    public string test;
    public string test2;
    public string test3;
    public string test4;
    //

    //-START AND UPDATE------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void Start()
    {
        InitializeCipher(); 
        InitializeCipherHint();
        InitializePictureHint();
        InitializeAnswer();
        InitializeKeyboardLetters();
        InitializeLetterSlots();
        //
        test = GetAtbash("monkey");
        test2 = GetCaeserForward(test, 2);
        test3 = GetCaeserBackward(test2, 2);
        test4 = GetAtbash(test3);
        //
    }

    void Update()
    {
        
    }

    //-SETTERS AND GETTERS------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    string GetAtbash(string input) // A-Z Inverse
    {
        string s = input.ToLower();
        var charArray = s.ToCharArray();
        for (int i = 0; i < charArray.Length; i++)
        {
            char c = charArray[i];

            if (c >= 'a' && c <= 'z')
            {
                charArray[i] = (char)(96 + (123 - c));
            }
        }
        return new string(charArray);
    }

    string GetCaeserForward(string input, int shift) // shift forward in the alphabet
    {
        string s = input.ToLower();
        char[] charArray = s.ToCharArray();
        for (int i = 0; i < charArray.Length; i++)
        {
            char c = charArray[i]; // letter
            c = (char)(c + shift); // add shift
            if(c > 'z')
            {
                c = (char)(c - 26);
            }
            charArray[i] = c;
        }
        return new string(charArray);
    }

    string GetCaeserBackward(string input, int shift) // shift backward in the alphabet
    {
        string s = input.ToLower();
        char[] charArray = s.ToCharArray();
        for (int i = 0; i < charArray.Length; i++)
        {
            char c = charArray[i]; // letter
            c = (char)(c - shift); // add shift
            if (c < 'a')
            {
                c = (char)(c + 26);
            }
            charArray[i] = c;
        }
        return new string(charArray);
    }

    //-FUNCTIONS------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void InitializeCipher()
    {

    }

    void InitializeCipherHint()
    {

    }

    void InitializePictureHint()
    {

    }

    void InitializeAnswer()
    {

    }

    void InitializeKeyboardLetters()
    {

    }

    void InitializeLetterSlots()
    {

    }

    
}
