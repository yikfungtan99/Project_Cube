using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

// ReSharper disable once InconsistentNaming
public class NetworkUI : MonoBehaviourPun
{
    private NetworkManager _networkManager;
    [SerializeField] private GameObject networkCanvas;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] private GameObject waitingPanel;
    [SerializeField] private GameObject joinPanel;
    [SerializeField] private TextMeshProUGUI roomNumber;
    [SerializeField] private TextMeshProUGUI playerNumber;

    [SerializeField] private Button btnStartGame;
    
    // Start is called before the first frame update
    private void Start()
    {
        _networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
        InitEvents();
    }

    private void InitEvents()
    {
        Debug.Log("Init Event");
        _networkManager.OnConnectedToServer += EnableNetworkCanvas;
        _networkManager.OnConnectedToRoom += WaitingScreen;
        _networkManager.OnDisconnectedFromRoom += RoomScreen;
        _networkManager.OnRoomCreated += UpdateRoomNumber;
        _networkManager.OnPlayerEnter += WaitingScreen;
        _networkManager.OnPlayerLeft += WaitingScreen;
    }
    
    private void EnableNetworkCanvas(object sender, EventArgs e)
    {
        networkCanvas.SetActive(true);
    }

    private void WaitingScreen(object sender, EventArgs e)
    {
        if(joinPanel.activeInHierarchy) joinPanel.SetActive(false);
        waitingPanel.SetActive(true);
        roomNumber.text = PhotonNetwork.CurrentRoom.Name;
        playerNumber.text = (PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers).ToString();
        btnStartGame.gameObject.SetActive(PhotonNetwork.LocalPlayer.ActorNumber == 1);
        roomPanel.SetActive(false);
    }

    private void RoomScreen(object sender, EventArgs e)
    {
        waitingPanel.SetActive(false);
        roomPanel.SetActive(true);
    }

    private void Test(object sender, EventArgs e)
    {
        print("Test");
    }

    private void JoinScreen(object sender, EventArgs e)
    {
        Debug.Log("Join Screen");
        ToggleJoinScreen();
    }

    //Toggle JoinPanel
    public void ToggleJoinScreen()
    {
        Debug.Log("JoinPanel");
        joinPanel.SetActive(!joinPanel.activeInHierarchy);
        joinPanel.GetComponent<RoomListing>().ClearListing();
    }
    
    private void UpdateRoomNumber(string roomNum)
    {
        roomNumber.text = roomNum;
    }

    private void OnDestroy()
    {

        if (!_networkManager) return;
        
        _networkManager.OnConnectedToServer -= EnableNetworkCanvas;
        _networkManager.OnConnectedToRoom -= WaitingScreen;
        _networkManager.OnDisconnectedFromRoom -= RoomScreen;
        _networkManager.OnRoomCreated -= UpdateRoomNumber;
        _networkManager.OnPlayerEnter -= WaitingScreen;
        _networkManager.OnPlayerLeft -= WaitingScreen;
    }
}
