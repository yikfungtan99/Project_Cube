using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerCube : MonoBehaviourPun
{
    public PlayerCube otherCube;
    [SerializeField] private PuzzleModule[] modules;

    private void Start()
    {
        for (int i = 0; i < modules.Length; i++)
        {
            if (modules[i].GetComponent<IInteractable>() != null)
            {
                modules[i].GetComponent<IInteractable>().OnInteracted += Action;
            }
            
            modules[i].PuzzleId = i;
        }
    }

    private void Update()
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

    public void Action(object sender, OnInteractedEventArgs e)
    {
        this.photonView.RPC("RpcAction", RpcTarget.Others, e.Id);
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
            modules[i].GetComponent<IInteractable>().OnInteracted -= Action;
        }
    }
}