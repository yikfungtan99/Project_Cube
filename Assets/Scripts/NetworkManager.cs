using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public event EventHandler OnConnectedToServer;
    public event EventHandler OnConnectedToRoom;
    public event EventHandler OnDisconnectedFromRoom;
    public event CreatedRoom OnRoomCreated;
    public delegate void CreatedRoom(string roomNum);
    public event EventHandler OnJoinButtonPressed;

    private bool _creatingRoom = false;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        print("Connecting to Master");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    #region  Network Functions

    private int RandRoomNumber()
    {
        return Random.Range(0, 999);
    }

    public void HostButton()
    {
        JoinLobby();
        _creatingRoom = true;
    }

    private void CreateRoom()
    {
        RoomOptions options = new RoomOptions();
        options.IsVisible = true;
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(RandRoomNumber().ToString(), options, TypedLobby.Default);
    }

    public void JoinLobby()
    { 
        PhotonNetwork.JoinLobby();
    }

    public void JoinButton()
    {
        JoinLobby();
        OnJoinButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    public void DisconnectRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    #endregion

    #region Network Logic

    private bool CheckPlayerCount()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;
    }
    
    private void LoadNextLevel()
    {
        PhotonNetwork.LoadLevel(1);
    }
    #endregion

    #region  Callbacks

    public override void OnConnectedToMaster()
    {
        print("Connected to Master");
        OnConnectedToServer?.Invoke(this, EventArgs.Empty);
    }

    public override void OnJoinedLobby()
    {
        print("Connected to Lobby");
        if(_creatingRoom) CreateRoom();
    }

    public override void OnLeftLobby()
    {
        print("Left lobby");
        _creatingRoom = false;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from master due to " + cause.ToString());
        OnDisconnectedFromRoom?.Invoke(this, EventArgs.Empty);
        _creatingRoom = false;
    }
    
    public override void OnCreatedRoom()
    {
        print("Created Room");
        OnRoomCreated?.Invoke(PhotonNetwork.CurrentRoom.Name);
        _creatingRoom = false;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Failed to Create room due to: " + message);
        _creatingRoom = false;
    }

    public override void OnJoinedRoom()
    {
        print("Joined Room");
        OnConnectedToRoom?.Invoke(this, EventArgs.Empty);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Failed to Join room due to: " + message);
        OnDisconnectedFromRoom?.Invoke(this, EventArgs.Empty);
        _creatingRoom = false;
    }

    public override void OnLeftRoom()
    {
        print("Disconnected from Room");
        OnDisconnectedFromRoom?.Invoke(this, EventArgs.Empty);
        _creatingRoom = false;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("Player has entered Room");
        if(CheckPlayerCount()) LoadNextLevel();
    }

    #endregion

}
