using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{

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
        SceneManager.LoadScene (3);
    }

    public void backButton()
    {
        SceneManager.LoadScene (0);
    }
}
