using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public event EventHandler OnConnectedToServer;
    public event EventHandler OnConnectedToRoom;
    public event EventHandler OnDisconnectedFromRoom;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        print("Connecting to Master");
        // PhotonNetwork.NickName = NetworkSingleton.NetworkSettings.NickName;
        // PhotonNetwork.GameVersion = NetworkSingleton.NetworkSettings.GameVersion;
        PhotonNetwork.NickName = "Random";
        PhotonNetwork.GameVersion = "0.0.0";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    #region  Network Functions

    public void CreateRoom()
    {
        //Failsafe if player already in Room
        if (PhotonNetwork.InRoom) return;
        
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom("basic", options, TypedLobby.Default);
    }

    public void JoinRoom()
    {
        //Failsafe if player already in Room
        if (PhotonNetwork.InRoom) return;
        PhotonNetwork.JoinRoom("basic");
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
        base.photonView.RPC("RPCLoadNextLevel", RpcTarget.All);
    }

    [PunRPC]
    private void RPCLoadNextLevel()
    {
        PhotonNetwork.LoadLevel(1);
    }

    #endregion

    #region  Callbacks

    public override void OnConnectedToMaster()
    {
        print("Connected to Master");
        print(PhotonNetwork.LocalPlayer.NickName);
        OnConnectedToServer?.Invoke(this, EventArgs.Empty);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from master due to " + cause.ToString());
        OnDisconnectedFromRoom?.Invoke(this, EventArgs.Empty);
    }
    
    public override void OnCreatedRoom()
    {
        print("Created Room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Failed to Create room due to: " + message);
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
    }

    public override void OnLeftRoom()
    {
        print("Disconnected from Room");
        OnDisconnectedFromRoom?.Invoke(this, EventArgs.Empty);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("Player has entered Room");
        if(CheckPlayerCount()) RPCLoadNextLevel();
    }

    #endregion

}
