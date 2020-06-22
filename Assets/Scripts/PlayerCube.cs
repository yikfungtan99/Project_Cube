using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class PlayerCube : MonoBehaviourPun
{
    public PlayerCube otherCube;
    [SerializeField] private PuzzleModule[] modules;
    private bool _allPuzzleGenerated = false;

    private void Start()
    {
        // for (int i = 0; i < modules.Length; i++)
        // {
        //     if (modules[i].GetComponent<IInteractable>() != null)
        //     {
        //         modules[i].GetComponent<IInteractable>().OnInteracted += Action;
        //     }
        // }
    }

    private void Update()
    {
        if (!otherCube)
        {
            LoadCube();
        }
        else
        {
            GeneratePuzzle();
        }
    }

    private void LoadCube()
    {
        if (!otherCube)
        {
            AssignOtherCube();
        }
    }

    //Find better way to Assign network instantiated object
    private void AssignOtherCube()
    {
        foreach (var view in PhotonNetwork.PhotonViews)
        {
            if (!view.IsMine)
            {
                otherCube = view.GetComponent<PlayerCube>();
                
                //This is stupid 
                otherCube.otherCube = this;
            }
        }
    }

    //Init the puzzle generate
    private void GeneratePuzzle()
    {
        if (_allPuzzleGenerated) return;

        //Only host can generate puzzle to determine randomness
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 6; i++)
            {
                PuzzleModuleData moduleDataInstance = GeneratePuzzleModuleData();
                modules[i].moduleData = moduleDataInstance;
                modules[i].SpawnPuzzle();
                moduleDataInstance.ReverseRole();
                otherCube.modules[i].moduleData = moduleDataInstance;
                otherCube.modules[i].SpawnPuzzle();
            }

            _allPuzzleGenerated = true;
        }
    }

    //Random Puzzle Generation
    private PuzzleModuleData GeneratePuzzleModuleData()
    {
        PuzzleTypes puzzleGenType = (PuzzleTypes) Random.Range(0, Enum.GetNames(typeof(PuzzleTypes)).Length);
        int puzzleGenVar = Random.Range(0, 2);
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

    public void Action(object sender, OnInteractedEventArgs e)
    {
        this.photonView.RPC("RpcAction", RpcTarget.All, e.Id);
    }

    [PunRPC]
    void RpcAction(int id)
    {
        Debug.Log(id);
        otherCube.modules[id + 1].GetComponent<IReactable>().ReAct();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < modules.Length; i++)
        {
            if (modules[i].GetComponent<IInteractable>() != null)
            {
                modules[i].GetComponent<IInteractable>().OnInteracted -= Action;
            }
        }
    }
}