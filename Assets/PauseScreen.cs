using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    
    private NetworkManager _networkManager;

    // Start is called before the first frame update
    private void Start()
    {
        _networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    public void DisconnectButton()
    {
        _networkManager.DisconnectGame();
    }
}
