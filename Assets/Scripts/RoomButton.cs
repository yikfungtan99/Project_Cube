using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNum;
    public RoomInfo RoomInfo { get; private set; }

    private string _roomName;

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        roomNum.text = roomInfo.Name;
        _roomName = roomInfo.Name;
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_roomName);
    }
}
