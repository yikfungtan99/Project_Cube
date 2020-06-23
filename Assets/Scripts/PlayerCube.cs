using System;
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

    private IEnumerator Start()
    {
        // for (int i = 0; i < modules.Length; i++)
        // {
        //     if (modules[i].GetComponent<IInteractable>() != null)
        //     {
        //         modules[i].GetComponent<IInteractable>().OnInteracted += Action;
        //     }
        // }
        while (!otherCube)
        {
            yield return null;
        }
        print("Start");
        GenerateRandomPuzzle();
    }

    //Archived: Now used IPunInstantiateMagicCallback to called for spawn system to help init other cube
    
    // private void LoadCube()
    // {
    //     if (!otherCube)
    //     {
    //         AssignOtherCube();
    //     }
    // }
    //
    // //Find better way to Assign network instantiated object
    // private void AssignOtherCube()
    // {
    //     print("Finding Cube");
    //     foreach (var view in PhotonNetwork.PhotonViews)
    //     {
    //         if (!view.IsMine)
    //         {
    //             otherCube = view.GetComponent<PlayerCube>();
    //             
    //             //This is stupid 
    //             otherCube.otherCube = this;
    //         }
    //     }
    // }

    //Init the puzzle generate
    private void GenerateRandomPuzzle()
    {
        if (!photonView.IsMine) return;
        //Only host can generate puzzle to determine randomness
        if (!PhotonNetwork.IsMasterClient) return;
        
        if (_allPuzzleGenerated) return;
        print("generating puzzle");
        
        for (int i = 0; i < 6; i++)
        {
            PuzzleModuleData moduleDataInstance = GeneratePuzzleModuleData();
            this.photonView.RPC("RpcSpawnPuzzle", RpcTarget.All, i,(int)moduleDataInstance.PuzzleType, moduleDataInstance.PuzzleVariation, moduleDataInstance.PuzzleRole);
        }

        _allPuzzleGenerated = true;
    }

    [PunRPC]
    private void RpcSpawnPuzzle(int id,int pt, int pv, int pr)
    {
        modules[id].SpawnPuzzle(pt, pv, pr);
        otherCube.modules[id].SpawnPuzzle(pt, pv, -pr);
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

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        SpawnSystem ss = GameObject.FindWithTag("SpawnSystem").GetComponent<SpawnSystem>();
        ss.currentCubes.Add(info.photonView.GetComponent<PlayerCube>());
        ss.AssignCube();
    }
}