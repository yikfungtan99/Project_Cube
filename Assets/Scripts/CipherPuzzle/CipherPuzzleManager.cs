
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public static class ListExensions // custom extension to list
{
    public static void AddMany<T>(this List<T> list, params T[] elements) // function for adding multiple elements to list in one line
    {
        list.AddRange(elements);
    }
}

public class CipherPuzzleManager : PuzzleManager
{
    [System.Serializable]
    public class CipherPuzzle
    {
        public int cipher; // store which cipher to use (1 = Atbash, 2 = CaeserForward, 3 = CaeserBackward)
        public int cipherHint; // store which cipher hint to use based on cipher chosen (if atbash, randomly select atbash hint preset (1 = AtbashHint1, 2 = AtbashHint2, etc)
        public int pictureHint; // store which picture hint to use (1 = monkeyPicture, 2 = birdPicture, 3 = tigerPicture)
        public string answer; // store which answer is required to solve puzzle based on picture stored (if monkeyPicture, 1 = monkey, 2 = chimp)
        public List<CipherButtons> buttons; // for initializing buttons
        public List<LetterSlots> letterSlots; // for initializing slots
        public string slotInput; // player input to compare to answer (if slotInput == answer, Win)
        public int test;
    }
    public CipherPuzzle cipherPuzzle;
    public bool isReactor = true;
    private bool _started = false;
    bool runFix = false;
    private SolveButton solveButton;


    //-START AND UPDATE------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public override void PuzzleStart()
    {
        if (_started) return;
        InitSeed();
        _started = true;
        InitializeSolveButton();
    }

    //-SETTERS AND GETTERS------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    #region Setters And Getters
    private string GetAtbash(string input) // A-Z Inverse
    {
        string s = input.ToLower();
        char[] charArray = s.ToCharArray();
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

    private string GetCaeserForward(string input, int shift) // shift forward in the alphabet
    {
        string s = input.ToLower();
        char[] charArray = s.ToCharArray();
        for (int i = 0; i < charArray.Length; i++)
        {
            char c = charArray[i]; // letter
            c = (char)(c + shift); // add shift
            if (c > 'z')
            {
                c = (char)(c - 26);
            }
            charArray[i] = c;
        }
        return new string(charArray);
    }

    private string GetCaeserBackward(string input, int shift) // shift backward in the alphabet
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

    public string Encode(string s)
    {
        if (cipherPuzzle.cipher == 0)
            return GetAtbash(s);
        else if (cipherPuzzle.cipher == 1)
            return GetCaeserForward(s, 3);
        else if (cipherPuzzle.cipher == 2)
            return GetCaeserBackward(s, 3);
        else
            return s;
    }

    private string Decode(string s)
    {
        if (cipherPuzzle.cipher == 0)
            return GetAtbash(s);
        else if (cipherPuzzle.cipher == 1)
            return GetCaeserBackward(s, 3); // reminder this is SUPPOSED to be backward
        else if (cipherPuzzle.cipher == 2)
            return GetCaeserForward(s, 3);
        else
            return s;
    }


    #endregion
    //-FUNCTIONS------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    //Archived============================================================================================================
    //RPC this
    public void KeyboardButtonPress(string s) // turn this into ReAct()?
    {
        if (!isReactor)
        {
            _puzzleModule.playerCube.CipherPuzzleButton(_puzzleModule.PuzzleId, s);
        }
        cipherPuzzle.slotInput += Decode(s);

        if (cipherPuzzle.slotInput.Length > cipherPuzzle.answer.Length) // clear slots if full
        {
            cipherPuzzle.slotInput = "";

            if (isReactor)
            {
                Clear();
            }
        }
        else
        {
            if (isReactor)
            {
                char[] charArray = cipherPuzzle.slotInput.ToCharArray();
        
                for (int i = 0; i < charArray.Length; i++)
                {
                   cipherPuzzle.letterSlots[i].SetSlot(charArray[i].ToString());
                }
                LetterSlotFix();
            }
        }
        
        if (!isReactor)
        {
            //CheckWin();
        }
    }

    void LetterSlotFix()
    {
        if(!runFix)
        {
            LetterSlots[] slots = GetComponentsInChildren<LetterSlots>();
            for (int i = cipherPuzzle.answer.Length; i < slots.Length; i++)
            {
                slots[i].ChangeSlotState(true);
            }
            runFix = true;
        }
    }

    public void Clear()
    {
        _puzzleModule.playerCube.CipherClear(_puzzleModule.PuzzleId);
        
        for (int i = 0; i < cipherPuzzle.answer.Length; i++)
        {
            cipherPuzzle.letterSlots[i].ClearSlot();
        }
        
        cipherPuzzle.slotInput = "";
    }

    public void ClearSlot(){

        print("ClearSlot");
        //if (!isReactor)
        //{
            cipherPuzzle.slotInput = "";
        //}
        // else
        // {
        //     for (int i = 0; i < cipherPuzzle.answer.Length; i++)
        //     {
        //         cipherPuzzle.letterSlots[i].ClearSlot();
        //     }
        // }
    }
    private void InitSeed()
    {
        CipherHint cipherHint = GetComponentInChildren<CipherHint>();
        if (isReactor) return;
        int seed = Random.Range(0, 999);
        
        InitializeCipherPuzzle(seed);
    }

    //Initializer
    void InitializeSolveButton()
    {
        solveButton = GetComponentInChildren<SolveButton>();
        if(solveButton != null)
            solveButton.SetManager(this);
    }

    public void InitializeCipherPuzzle(int seed)
    {
        Random.InitState(seed);
        //set values
        RandomizeCipher();
        RandomizeCipherHint();
        RandomizePictureHint();
        RandomizeAnswer();
        
        
        if (!isReactor)
        {
            InitializeKeyboard();
            _puzzleModule.playerCube.CipherPuzzleRpc(_puzzleModule.PuzzleId, seed);
            // ocpm = GameObject.Find("Cipher0Reactor").GetComponent<CipherPuzzleManager>();
            // ocpm.InitializeCipherPuzzle(seed);
        }
        else
        {
            InitializeCipherHint();
            InitializeLetterSlots();
        }
        
        //InitializeReactor(cipherPuzzle.cipher, cipherPuzzle.cipherHint, cipherPuzzle.pictureHint, cipherPuzzle.answer);

        // else
        // {
        //     //Remove this======================================================================================================
        //     isReactor = true;
        //     //cpm = _puzzleModule.puzzleManager as CipherPuzzleManager; // yik fung like this ah?
        //     cpm = GameObject.Find("Cipher0Reactor").GetComponent<CipherPuzzleManager>(); // @yik fung change this to how networking get the other cube component idk xd
        //     StartCoroutine(InitializeReactor());
        // }
    }

    //Receiver

    #region Randomization

    private void RandomizeCipher() // randomize which cipher to use
    {
        cipherPuzzle.test = Random.Range(0, 999);
        int i = UnityEngine.Random.Range(0, 3);
        if (i == 0)
            cipherPuzzle.cipher = 0; // atbash
        else if (i == 1)
            cipherPuzzle.cipher = 1; // caeser forward
        else if (i == 2)
            cipherPuzzle.cipher = 2; // caeser backward
    }

    private void RandomizeCipherHint() // requires cipher to be randomized 1st
    {
        int i = UnityEngine.Random.Range(0, 3);
        
        //=======================================================================
        if (cipherPuzzle.cipher == 0) // if atbash
        {
            if (i == 0)
                cipherPuzzle.cipherHint = 0; // atbash cipher hint 1
            else if (i == 1)
                cipherPuzzle.cipherHint = 1; // atbash cipher hint 2
            else if (i == 2)
                cipherPuzzle.cipherHint = 2; // atbash cipher hint 3
        }
        else if (cipherPuzzle.cipher == 1) // if caeser forward
        {
            if (i == 0)
                cipherPuzzle.cipherHint = 3; // caeser cipher forward hint 1
            else if (i == 1)
                cipherPuzzle.cipherHint = 4; // caeser cipher forward hint 2
            else if (i == 2)
                cipherPuzzle.cipherHint = 5; // caeser cipher forward hint 3
        }
        else if (cipherPuzzle.cipher == 2) // if caeser backward
        {
            if (i == 0)
                cipherPuzzle.cipherHint = 6; // caeser cipher backward hint 1
            else if (i == 1)
                cipherPuzzle.cipherHint = 7; // caeser cipher backward hint 2
            else if (i == 2)
                cipherPuzzle.cipherHint = 8; // caeser cipher backward hint 3
        }
        //=======================================================================

    }

    private void RandomizePictureHint() // randomize which picture to use
    {
        int i = UnityEngine.Random.Range(0, 3);
        if (i == 0)
            cipherPuzzle.pictureHint = 0; // Picture of Bird
        else if (i == 1)
            cipherPuzzle.pictureHint = 1; // Picture of Dog
        else if (i == 2)
            cipherPuzzle.pictureHint = 2; // Picture of Tree
        
        InitializePictureHint();
    }

    private void RandomizeAnswer() // requires picturehint to be initialized 1st
    {
        int i = UnityEngine.Random.Range(0, 3);
        if (cipherPuzzle.pictureHint == 0)
        {
            if (i == 0)
                cipherPuzzle.answer = "bird";
            else if (i == 1)
                cipherPuzzle.answer = "wings";
            else if (i == 2)
                cipherPuzzle.answer = "fly";
        }
        else if (cipherPuzzle.pictureHint == 1)
        {
            if (i == 0)
                cipherPuzzle.answer = "dog";
            else if (i == 1)
                cipherPuzzle.answer = "bark";
            else if (i == 2)
                cipherPuzzle.answer = "pet";
        }
        else if (cipherPuzzle.pictureHint == 2)
        {
            if (i == 0)
                cipherPuzzle.answer = "tree";
            else if (i == 1)
                cipherPuzzle.answer = "plant";
            else if (i == 2)
                cipherPuzzle.answer = "wood";
        }
    }
    #endregion

    #region Interactor Initialization

    private void InitializePictureHint()
    {
        PictureHint pictureHint = GetComponentInChildren<PictureHint>();
        if (pictureHint != null)
            pictureHint.transform.GetChild(cipherPuzzle.pictureHint).gameObject.SetActive(true);
    }

    private void InitializeKeyboard()
    {
        CipherButtons[] cipherButtons = GetComponentsInChildren<CipherButtons>();
        if (cipherButtons.Length > 0) // check if not empty
        {
            for (int i = 0; i < cipherButtons.Length; i++)
            {
                cipherButtons[i].cpm = this;
                cipherPuzzle.buttons.Add(cipherButtons[i]);
            }
            // bunch of alphabet stuff
            List<string> alphabet = new List<string>();
            alphabet.AddMany<string>("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T,", "U", "V", "W", "X", "Y", "Z");
            List<string> remainingAlphabet = new List<string>();
            List<string> answerLetters = new List<string>();
            remainingAlphabet.AddRange(alphabet);

            int remainingKeySlots = cipherPuzzle.buttons.Count;
            int answerLength = cipherPuzzle.answer.Length;

            string[] letters = new string[cipherPuzzle.answer.Length];
            for (int i = 0; i < answerLength; i++)
            {
                letters[i] = cipherPuzzle.answer[i].ToString();
            }

            for (int i = 0; i < letters.Length; i++)
            {
                answerLetters.Add(letters[i].ToUpper());
                if (remainingAlphabet.Contains(letters[i].ToUpper()))
                {
                    remainingAlphabet.Remove(letters[i].ToUpper());
                }
            }

            remainingKeySlots = remainingKeySlots - answerLetters.Count;

            // add answer letters to keyboard buttons
            List<int> listNumbers = new List<int>();
            int number;
            for (int i = 0; i < answerLetters.Count; i++)
            {
                do
                {
                    number = UnityEngine.Random.Range(0, cipherPuzzle.buttons.Count);
                } while (listNumbers.Contains(number));
                listNumbers.Add(number);
                cipherPuzzle.buttons[number].SetLetter(answerLetters[i]);
            }
            //add fill remaining slots with letters
            int alphabetNumber;
            for (int i = 0; i < remainingKeySlots; i++)
            {
                do
                {
                    number = UnityEngine.Random.Range(0, cipherPuzzle.buttons.Count);
                    alphabetNumber = UnityEngine.Random.Range(0, remainingAlphabet.Count);
                } while (listNumbers.Contains(number));
                listNumbers.Add(number);
                cipherPuzzle.buttons[number].SetLetter(remainingAlphabet[alphabetNumber]);
            }
        }
    }
    #endregion

    #region Reactor Initialization

    private void InitializeCipherHint()
    {
        CipherHint cipherHint = GetComponentInChildren<CipherHint>();
        if (cipherHint != null)
            cipherHint.transform.GetChild(cipherPuzzle.cipherHint).gameObject.SetActive(true);
    }

    private void InitializeLetterSlots()
    {
        LetterSlots[] slots = GetComponentsInChildren<LetterSlots>();
        //cipherPuzzle.answer.Replace(" ", string.Empty);
        cipherPuzzle.answer.Trim();
        if (slots.Length > 0) // check if not empty
        {
            for (int i = 0; i < cipherPuzzle.answer.Length; i++)
            {
                slots[i].ChangeSlotState(false);
                cipherPuzzle.letterSlots.Add(slots[i]);
            }
        }
    }
    #endregion

    public override void CheckWin() 
    {
        if(cipherPuzzle.slotInput == cipherPuzzle.answer)
        {
            _puzzleModule.ModuleComplete();
        }
    }
}
