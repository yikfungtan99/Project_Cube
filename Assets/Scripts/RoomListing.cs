using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomListing : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform context;
    [SerializeField] private RoomButton _roomButton;

    private List<RoomButton> _buttons = new List<RoomButton>();

    private void Start()
    {

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateList(roomList);
    }

    private void UpdateList(List<RoomInfo> roomList)
    {
        Debug.Log("UpdateRoom");
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = _buttons.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(_buttons[index].gameObject);
                    _buttons.RemoveAt(index);
                }
            }
            else
            {
                RoomButton roomButton = Instantiate(_roomButton, context);
                if (roomButton != null)
                {
                    roomButton.SetRoomInfo(info);
                    _buttons.Add(roomButton);
                }
            }
        }
    }

    public void ClearListing()
    {
        print("Cleared Listing");
        _buttons.Clear();
        for (int i = 0; i < context.childCount; i++)
        {
            Destroy(context.GetChild(i).gameObject);
        }
    }
}