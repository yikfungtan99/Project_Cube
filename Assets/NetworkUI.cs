using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ReSharper disable once InconsistentNaming
public class NetworkUI : MonoBehaviour
{
    private NetworkManager _networkManager;
    [SerializeField] private GameObject networkCanvas;
    
    // Start is called before the first frame update
    private void Start()
    {
        _networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
        InitEvents();
    }

    private void InitEvents()
    {
        _networkManager.OnConnectedToServer += EnableNetworkCanvas;
        _networkManager.OnConnectedToRoom += WaitingScreen;
        _networkManager.OnDisconnectedFromRoom += RoomScreen;
    }
    
    private void EnableNetworkCanvas(object sender, EventArgs e)
    {
        networkCanvas.SetActive(true);
    }

    private void WaitingScreen(object sender, EventArgs e)
    {
        networkCanvas.transform.GetChild(0).gameObject.SetActive(false);
        networkCanvas.transform.GetChild(1).gameObject.SetActive(true);
    }

    private void RoomScreen(object sender, EventArgs e)
    {
        networkCanvas.transform.GetChild(0).gameObject.SetActive(true);
        networkCanvas.transform.GetChild(1).gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _networkManager.OnConnectedToServer -= EnableNetworkCanvas;
        _networkManager.OnConnectedToRoom -= WaitingScreen;
        _networkManager.OnDisconnectedFromRoom -= RoomScreen;
    }
}
