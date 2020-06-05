﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

// ReSharper disable once InconsistentNaming
public class NetworkUI : MonoBehaviour
{
    private NetworkManager _networkManager;
    [SerializeField] private GameObject networkCanvas;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] private GameObject waitingPanel;
    [SerializeField] private GameObject joinPanel;
    [SerializeField] private TextMeshProUGUI roomNumber;

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
        _networkManager.OnRoomCreated += UpdateRoomNumber;
        _networkManager.OnJoinButtonPressed += JoinScreen;
    }
    
    private void EnableNetworkCanvas(object sender, EventArgs e)
    {
        networkCanvas.SetActive(true);
    }

    private void WaitingScreen(object sender, EventArgs e)
    {
        waitingPanel.SetActive(true);
        roomPanel.SetActive(false);
    }

    private void RoomScreen(object sender, EventArgs e)
    {
        waitingPanel.SetActive(false);
        roomPanel.SetActive(true);
    }

    private void JoinScreen(object sender, EventArgs e)
    {
        ToggleJoinScreen();
    }

    //Toggle JoinPanel
    public void ToggleJoinScreen()
    {
        joinPanel.SetActive(!joinPanel.activeInHierarchy);
        joinPanel.GetComponent<RoomListing>().ClearListing();
    }
    
    private void UpdateRoomNumber(string roomNum)
    {
        roomNumber.text = roomNum;
    }

    private void OnDestroy()
    {
        _networkManager.OnConnectedToServer -= EnableNetworkCanvas;
        _networkManager.OnConnectedToRoom -= WaitingScreen;
        _networkManager.OnDisconnectedFromRoom -= RoomScreen;
        _networkManager.OnRoomCreated -= UpdateRoomNumber;
        _networkManager.OnJoinButtonPressed -= JoinScreen;
    }
}
