using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    private NetworkManager _networkManager;
    [SerializeField]private GameObject manualScreen;

    public void StartButton()
    {
        SceneManager.LoadScene (1);
    }

    public void SettingButton()
    {
        
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene(3);
    }

    public void BackButton()
    {
        _networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
        if (_networkManager)
        {
            _networkManager.DisconnectGame();
        }
    }

    public void CreditsBackButton()
    {
        SceneManager.LoadScene(0);
    }

    public void ManualButton()
    {
        manualScreen.SetActive(true);
    }

    public void ManualBackButton()
    {
        manualScreen.SetActive(false);
    }
}
