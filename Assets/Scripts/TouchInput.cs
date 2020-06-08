using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    [SerializeField] private Transform cubeTransform;
    [SerializeField] private float rotateSpd;
    private Vector3 _initPos;
    private Touch _touch;

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }

    void UpdateInput()
    {
        if (Input.touchCount <= 0) return;
        
        //Take the first touch, refreshes every frame
        _touch = Input.GetTouch(0);

        if (_touch.phase != TouchPhase.Moved) return;

        var rotation = cubeTransform.rotation;
        //rotation = Quaternion.Euler(_touch.deltaPosition.y * rotateSpd * Time.deltaTime, -_touch.deltaPosition.x* rotateSpd* Time.deltaTime, rotation.z);

        if (CheckTouchRotation())
        {
            rotation = Quaternion.Euler(0f,-_touch.deltaPosition.x* rotateSpd* Time.deltaTime,0f);
        }
        else
        {
            rotation = Quaternion.Euler(_touch.deltaPosition.y * rotateSpd * Time.deltaTime,0f,0f);
        }
        
        cubeTransform.rotation = rotation * cubeTransform.rotation;
    }

    private bool CheckTouchRotation()
    {
        bool deltaX;

        deltaX = Math.Abs(_touch.deltaPosition.x) > Math.Abs(_touch.deltaPosition.y);

        return deltaX;
    }
}
