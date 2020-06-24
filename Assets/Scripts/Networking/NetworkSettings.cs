using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/NetworkSettings")]
public class NetworkSettings : ScriptableObjectSingleton<NetworkSettings>
{
    // [SerializeField] private string gameVersion = " 0.0.0";
    //
    // public string GameVersion => gameVersion;
    //
    // [SerializeField] private static string _nickName = "RandomName";
    //
    // public string NickName
    // {
    //     get
    //     {
    //         int value = Random.Range(0, 999);
    //         return _nickName + value;
    //     }
    //     set => _nickName = value;
    // }

    public static string gameVersion = "0.0.0";

}
    
    

