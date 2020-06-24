using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "Network/NetworkSingleton")]
public class NetworkSingleton : ScriptableObjectSingleton<NetworkSingleton>
{
    [SerializeField] private NetworkSettings networkSettings;

    public static NetworkSettings NetworkSettings => Instance.networkSettings;
}
