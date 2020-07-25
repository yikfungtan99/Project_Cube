using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private CubeState cubeState;

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject manualScreen;

    [SerializeField] private Button btnChangeState;
    [SerializeField] private TextMeshProUGUI txtChangeState;
    [SerializeField] private Button btnCalibrate;

    //DO NOT need to use Update but I am lazy
    private void Update()
    {
        btnCalibrate.gameObject.SetActive(cubeState.currentState == CubeStates.Rotate);
    }

    public void UpdateStateText()
    {
        string nextState = "";
        
        switch (cubeState.currentState)
        {
            case CubeStates.Examine:
                nextState = "Rotate";
                break;
                
            case CubeStates.Rotate:
                nextState = "Examine";
                break;
        }

        txtChangeState.text = nextState;
    }
    public void BackButton()
    {
        pauseScreen.SetActive(false);
    }

    public void SettingButton()
    {
        pauseScreen.SetActive(true);
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
