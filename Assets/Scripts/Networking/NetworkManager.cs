using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region UI Events

    public event EventHandler OnConnectedToServer;
    public event EventHandler OnConnectedToRoom;
    public event EventHandler OnDisconnectedFromRoom;
    public event EventHandler OnPlayerEnter;
    public event EventHandler OnPlayerLeft;
    public event CreatedRoom OnRoomCreated;
    public delegate void CreatedRoom(string roomNum);

    #endregion
    
    private bool _creatingRoom = false;
    private bool _joiningRoom = false;
    private bool _inGame = false;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        ConnectToMaster();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #region  Network Functions

    //Connect To Project Cube's Photon
    private void ConnectToMaster()
    {
        print("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    #region OnClick Events

    //Host Button Logic
    public void HostButton()
    {
        JoinLobby();
        _creatingRoom = true;
    }
    
    //Join Button Logic
    public void JoinButton()
    {
        Debug.Log("Join Button");
        JoinLobby();
    }

    #endregion
    
    //Generate Room
    //Call when successfully joined a lobby
    private void CreateRoom()
    {
        RoomOptions options = new RoomOptions();
        options.IsVisible = true;
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(RandRoomNumber().ToString(), options, TypedLobby.Default);
    }
    
    //Generate Room Number
    private int RandRoomNumber()
    {
        return Random.Range(0, 999);
    }

    //Join a lobby if attempted to Create or Join room
    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    //Disconnect from Room
    public void DisconnectRoom()
    {
        if (!PhotonNetwork.InRoom) return;
        PhotonNetwork.LeaveRoom();
        DisconnectGame();
    }

    public void DisconnectGame()
    {
        PhotonNetwork.Disconnect();
        StartCoroutine(DisconnectAndReturn());
    }

    public static IEnumerator DisconnectAndReturn()
    {
        while (PhotonNetwork.IsConnected) yield return null;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        //SceneManager.LoadScene(1);
    }

    #endregion

    #region Network Logic

    private bool CheckPlayerCount()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    public void StartGame()
    {
        if (CheckPlayerCount() && PhotonNetwork.IsMasterClient)
        {
            LoadNextLevel();
        }
    }
    
    private void LoadNextLevel()
    {
        PhotonNetwork.LoadLevel(2);
        _inGame = true;
    }
    #endregion

    #region Callbacks

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
        OnPlayerEnter?.Invoke(this, EventArgs.Empty);
        //if(CheckPlayerCount() && !_inGame) ableToStartGame = true;
    }
    
    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        print("Player left room");

        if (!_inGame)
        {
            
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                OnPlayerLeft?.Invoke(this,EventArgs.Empty);
            }
            else
            {
                DisconnectRoom();
            }
        }
        else
        {
            DisconnectGame();
        }

        //if(!CheckPlayerCount() && _inGame) DisconnectGame();
    }

    #endregion
}
