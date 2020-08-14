using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    private NetworkManager _networkManager;
    [SerializeField] private GameObject manualScreen;
    [SerializeField] private List<GameObject> manualTextList;
    [SerializeField] private int currentPage = 0;

    public void StartButton()
    {
        SceneManager.LoadScene(1);
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

    /// <summary>
    /// Next button function for manual screen's next button
    /// </summary>
    public void ManualNextButton()
    {
        if(currentPage < manualTextList.Count - 1)
        {
            manualTextList[currentPage].SetActive(false);
            currentPage++;
            manualTextList[currentPage].SetActive(true);
        }
        else
        {
            manualTextList[currentPage].SetActive(false);
            currentPage = 0;
            manualTextList[currentPage].SetActive(true);
        }
       
}

    /// <summary>
    /// Previous button function for manual screen's previous button
    /// </summary>
    public void ManualPrevButton()
    {
        if (currentPage <= 0)
        {
            manualTextList[0].SetActive(false);
            currentPage = manualTextList.Count - 1;
            manualTextList[currentPage].SetActive(true);
        }
        else if (currentPage > 0)
        {
            manualTextList[currentPage].SetActive(false);
            currentPage--;
            manualTextList[currentPage].SetActive(true);
        }
    }
}
