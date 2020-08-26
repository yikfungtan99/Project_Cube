using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class WinLoseScreen : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        PhotonNetwork.Disconnect();
        StartCoroutine(DisconnectToMainMenu());
    }
    public static IEnumerator DisconnectToMainMenu()
    {
        while (PhotonNetwork.IsConnected) yield return null;

        SceneManager.LoadScene(0);
    }
}
