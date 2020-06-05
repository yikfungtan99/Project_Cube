using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/NetworkSettings")]
public class NetworkSettings : ScriptableObject
{
    [SerializeField] private string gameVersion = " 0.0.0";

    public string GameVersion => gameVersion;

    [SerializeField] private string nickName = "RandomName";

    public string NickName
    {
        get
        {
            int value = Random.Range(0, 999);
            return nickName + value;
        }
        set => nickName = value;
    }
}
    
    

