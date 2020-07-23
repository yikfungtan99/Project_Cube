using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class SimonSaysPuzzleManager : PuzzleManager
{
    public event EventHandler<Interactor.OnInteractedEventArgs> OnCalled;
    private List<int> curSequence = new List<int>();

    [SerializeField] private int startingLightNumber = 2;
    private int _numberOfLight;

    [SerializeField] private bool repeating = true;

    [SerializeField] private float cooldown = 2.0f;

    [SerializeField] private int numberOfLevels = 3;
    private int _currentLevel = 0;

    private List<int> _solvingSequence = new List<int>();
    private int _curCheckingNumber = 0;
    private int _solvedCount = 0;

    public override void PuzzleStart()
    {
        base.PuzzleStart();
        if (_puzzleModule.interactors.Count == 0) return;
        OnCalled += _puzzleModule.GetComponentInParent<PlayerCube>().Action;
        _numberOfLight = startingLightNumber;
        InitSequence();
    }
    
    // public override void Start()
    // {
    //     base.Start();
    //
    //     if (_puzzleModule.interactors.Count == 0) return;
    //     OnCalled += _puzzleModule.GetComponentInParent<PlayerCube>().Action;
    //     _numberOfLight = startingLightNumber;
    //     InitSequence();
    // }

    private void InitSequence()
    {
        RandomSequence();
        StartCoroutine(RunSequence());
    }

    private void RandomSequence()
    {
        for (int i = 0; i < _numberOfLight; i++)
        {
            int initInt = Random.Range(0, 4);
            curSequence.Add(initInt);
        }
    }

    private IEnumerator RunSequence()
    {
        while (repeating)
        {
            for (int i = 0; i < curSequence.Count; i++)
            {
                OnCalled?.Invoke(this, new Interactor.OnInteractedEventArgs{ModuleId = _puzzleModule.PuzzleId, ComponentId = curSequence[i]});
                yield return new WaitForSeconds(0.6f);
            }

            yield return new WaitForSeconds(2.0f);
        }
        
        for (int i = 0; i < curSequence.Count; i++)
        {
            OnCalled?.Invoke(this, new Interactor.OnInteractedEventArgs{ModuleId = _puzzleModule.PuzzleId, ComponentId = curSequence[i]});
            yield return new WaitForSeconds(0.6f);
        }
    }

    private void SetSequence()
    {
        PhotonView view = _puzzleModule.GetComponentInParent<PhotonView>();
        int seed = Random.Range(0, 999);
        view.RPC("RpcSetSequence", RpcTarget.All, seed);
    }

    [PunRPC]
    private void RpcSetSequence(int seed)
    {
        
    }

    //if the button press is the same button of the current Sequence
    public void PressedButton(object sender, SimonSaysButton.OnSimonSaysButtonPressedButtonEventArgs e)
    {
        _solvingSequence.Add(e.ComponentId);

        if (curSequence.Count == 0) return;
        if (_solvingSequence[_curCheckingNumber] == curSequence[_curCheckingNumber])
        {
            //Correct Number
            _solvedCount += 1;
            _curCheckingNumber += 1;

            if (_solvedCount == curSequence.Count)
            {
                //Correct combination
                Winning();
            }
        }
        else
        {
            if (_currentLevel != 0)
            {
                //Wrong Combination
                Losing();
            }
            else
            {
                _solvedCount = 0;
                _curCheckingNumber = 0;
                _solvingSequence.Clear();
                print("first lost not counted");
            }
        }
    }

    private void Losing()
    {
        Reset();
        if (!repeating) repeating = true;
        
        print("YOU LOST : WRONG COMBINATION");
        _numberOfLight = startingLightNumber;

        StartCoroutine(StartNewSequence());

        _currentLevel = 0;
    }

    private void Winning()
    {
        Reset();
        if (repeating) repeating = false;
        _currentLevel += 1;
        
        _numberOfLight += 1;
        //Init new sequence

        if (_currentLevel < numberOfLevels)
        {
            StartCoroutine(StartNewSequence());
        }
        else
        {
            print("You Win");
            _puzzleModule.ModuleComplete();
        }
        
    }

    IEnumerator StartNewSequence()
    {
        print("OnCooldown");
        yield return new WaitForSeconds(cooldown);
        print("Cooldown Complete");
        InitSequence();
    }

    private void Reset()
    {
        _solvedCount = 0;
        _curCheckingNumber = 0;
        _solvingSequence.Clear();
        curSequence.Clear();
    }

    private void ClearAnswer()
    {
        print("Clearing sequence");
        _solvingSequence.Clear();
    }
    
    //Interactor Solving part
    
    //and in the correct time frame
    //clear current sequence
    //clear repeating
    //re random sequence
    //InitSequence
}
