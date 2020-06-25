﻿using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class PlayerCube : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public PlayerCube otherCube;
    [SerializeField] private PuzzleModule[] modules;
    private bool _allPuzzleGenerated = false;

    //Using a coroutine as we need to wait until other cube is detected
    private IEnumerator Start()
    {
        //wait until other cube is detected
        while (!otherCube)
        {
            yield return null;
        }
        
        //Generate puzzle at Start
        GenerateRandomPuzzle();
    }

    #region  Puzzle Generation
    
    private void GenerateRandomPuzzle()
    {
        if (!photonView.IsMine) return;
        
        //Only host can generate puzzle to determine randomness
        if (!PhotonNetwork.IsMasterClient) return;
        
        if (_allPuzzleGenerated) return;
        //print("generating puzzle");
        
        for (int i = 0; i < 6; i++)
        {
            PuzzleModuleData moduleDataInstance = GeneratePuzzleModuleData();
            this.photonView.RPC("RpcSpawnPuzzle", RpcTarget.All, i,(int)moduleDataInstance.PuzzleType, moduleDataInstance.PuzzleVariation, moduleDataInstance.PuzzleRole);
        }

        this.photonView.RPC("RpcInitModuleEvents", RpcTarget.All);
        _allPuzzleGenerated = true;
    }

    //Called on the cubes to spawn the puzzle according to data given
    [PunRPC]
    private void RpcSpawnPuzzle(int id,int pt, int pv, int pr)
    {
        modules[id].SpawnPuzzle(pt, pv, pr);
        otherCube.modules[id].SpawnPuzzle(pt, pv, -pr);
    }

    [PunRPC]
    private void RpcInitModuleEvents()
    {
        InitModuleComponents();
    }

    //Random Puzzle Generation
    private PuzzleModuleData GeneratePuzzleModuleData()
    {
        PuzzleTypes puzzleGenType = (PuzzleTypes) Random.Range(0, Enum.GetNames(typeof(PuzzleTypes)).Length);
        int puzzleGenVar = 0;
        int puzzleGenRole = Random.Range(0, 2);

        if (puzzleGenRole == 0)
        {
            puzzleGenRole = -1;
        }
        else
        {
            puzzleGenRole = 1;
        }
        
        PuzzleModuleData generatedPuzzleModuleData = new PuzzleModuleData(puzzleGenType, puzzleGenVar, puzzleGenRole);
        return generatedPuzzleModuleData;
    }

    //Initialise components events
    //*Need to change to detect multiple Intractable
    private void InitModuleComponents()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < modules.Length; i++)
            {
                if (modules[i].GetComponentInChildren<IInteractable>() != null)
                {
                    modules[i].GetComponentInChildren<IInteractable>().OnInteracted += Action;
                }
            }
        }
        else
        {
            for (int i = 0; i < otherCube.modules.Length; i++)
            {
                if (otherCube.modules[i].GetComponentInChildren<IInteractable>() != null)
                {
                    otherCube.modules[i].GetComponentInChildren<IInteractable>().OnInteracted += Action;
                }
            }
        }
       
        
        print("Finished Initialisation of Module Components");
    }
    
    #endregion

    #region Info Sending
    
    public void Action(object sender, OnInteractedEventArgs e)
    {
        //print("Attempt to call Rpc");
        this.photonView.RPC("RpcAction", RpcTarget.All, e.Id);
    }

    [PunRPC]
    void RpcAction(int id)
    {
        //Need to change to detect multiple reactor
        if (otherCube.modules[id].GetComponentInChildren<IReactable>() != null)
        {
            otherCube.modules[id].GetComponentInChildren<IReactable>().ReAct();
        }
        else
        {
            modules[id].GetComponentInChildren<IReactable>().ReAct();
        }
        
    }

    #endregion
    
    private void OnDestroy()
    {
        for (int i = 0; i < modules.Length; i++)
        {
            //Need to change to detect multiple interactor
            if (modules[i].GetComponent<IInteractable>() != null)
            {
                modules[i].GetComponent<IInteractable>().OnInteracted -= Action;
            }
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        SpawnSystem ss = GameObject.FindWithTag("SpawnSystem").GetComponent<SpawnSystem>();
        ss.currentCubes.Add(info.photonView.GetComponent<PlayerCube>());
        ss.AssignCube();
    }
}