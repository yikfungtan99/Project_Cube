using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkTest : MonoBehaviourPun
{
    //Generate -> Send -> Show

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) GenerateRandomSeed();
    }

    private void GenerateRandomSeed()
    {
        print("Generating");
        int seed = Random.Range(0, 100);
        
        photonView.RPC("RpcSendSeed", RpcTarget.All, seed);
    }

    [PunRPC]
    private void RpcSendSeed(int seed)
    {
        print("Receiving RPC");
        ShowRandomNumber(seed);
    }

    private void ShowRandomNumber(int seed)
    {    
        print("Showing result");
        Random.InitState(seed);
        int number = Random.Range(0, 999);
        print(number.ToString());
    }
}
