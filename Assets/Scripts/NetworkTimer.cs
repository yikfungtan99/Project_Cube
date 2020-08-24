using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NetworkTimer : MonoBehaviour
{
    [SerializeField] private double timerTime;
    [SerializeField] private TextMeshProUGUI timerText;

    private double _curTime = 0;

    [SerializeField] private bool timerTickDown = true;

    private double _startTime = 0.0f;

    private bool _timerEnd = false;

    public event EventHandler OnTimerEnd;
    
    // Start is called before the first frame update
    void Start()
    {
        _startTime = PhotonNetwork.Time;
        if(!timerText) Debug.Log("No Timer Text UI Found");
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    private void Timer()
    {
        double timeDiff = PhotonNetwork.Time - _startTime;

        if (timerTickDown)
        {
            if (_timerEnd) return;
            _curTime = timerTime - timeDiff;

            if (_curTime <= 0.0)
            {
                _curTime = 0.0;
                _timerEnd = true;
                SceneManager.LoadScene(5);
                OnTimerEnd?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            _curTime = timeDiff;
        }

        if (timerText)
        {
            ShowOnTextUI();
        }
    }

    private void ShowOnTextUI()
    {
        timerText.text = _curTime.ToString("F2");
    }
}
