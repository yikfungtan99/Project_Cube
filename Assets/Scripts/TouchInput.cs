﻿using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

[RequireComponent(typeof(CubeState))]
public class TouchInput : MonoBehaviour
{
    public bool useOnlyTouch = false;

    [SerializeField] private Camera mainCam;
    [SerializeField] private CubeState cubeState;
    private CubeStates _currentCubeState;
    
    public Transform cubeTransform;
    [SerializeField] private float rotateSpd;
    private Vector3 _initPos;
    private Touch _touch;
    
    private float _mouseDirX;
    private float _mouseDirY;

    private Vector2 _mouseDelta;

    [SerializeField] private float sensitivityThreshold;
    private bool _resetPending = true;
    private bool isDragging;

    float _rotX = 0;
    float _rotY = 0;
    
    private Quaternion _targetRotation;
    private Quaternion _prevRotation;

    private void Start()
    {
        if (!cubeTransform) cubeTransform = GameObject.FindWithTag("SpawnSystem").GetComponent<SpawnSystem>().playerCube.transform;
        cubeState = GetComponent<CubeState>();

        Input.simulateMouseWithTouches = true;

        _prevRotation = cubeTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCubeState();
        UpdateInputUsingTouchScreen();
        UpdateInputUsingMouse();
        TapOnInteractables();
    }

    private void UpdateCubeState()
    {
        _currentCubeState = cubeState.currentState;
    }

    void UpdateInputUsingTouchScreen()
    {
        if (_currentCubeState != CubeStates.Rotate) return;
        if (!useOnlyTouch) return;
        
        if (Input.touchCount <= 0) return;

        for (int i = 0; i < Input.touchCount; i++)
        {
            //Take the first touch, refreshes every frame
            _touch = Input.GetTouch(i);
        }

        if (_touch.phase != TouchPhase.Moved) return;

        var rotation = cubeTransform.rotation;
        //rotation = Quaternion.Euler(_touch.deltaPosition.y * rotateSpd * Time.deltaTime, -_touch.deltaPosition.x* rotateSpd* Time.deltaTime, rotation.z);

        if (CheckTouchDelta())
        {
            rotation = Quaternion.Euler(0f,-_touch.deltaPosition.x* rotateSpd* Time.deltaTime,0f);
        }
        else
        {
            rotation = Quaternion.Euler(_touch.deltaPosition.y * rotateSpd * Time.deltaTime,0f,0f);
        }
        
        cubeTransform.rotation = rotation * cubeTransform.rotation;
    }

    // private void UpdateInputUsingMouse()
    // {
    //     if (_currentCubeState != CubeStates.Rotate) return;
    //     if (useOnlyTouch) return;
    //     if (Input.GetMouseButtonDown(0)) _initPos = Input.mousePosition;
    //
    //     if (!Input.GetMouseButton(0)) return;
    //     
    //     _mouseDelta = Input.mousePosition - _initPos;
    //     
    //     if (_mouseDelta.x > 0)
    //     {
    //         _mouseDirX = 1;
    //     }
    //     else
    //     {
    //         _mouseDirX = -1;
    //     }
    //
    //     if (_mouseDelta.y > 0)
    //     {
    //         _mouseDirY = 1;
    //     }
    //     else
    //     {
    //         _mouseDirY = -1;
    //     }
    //
    //     var rotation = cubeTransform.rotation;
    //     
    //     if (CheckMouseDelta())
    //     {
    //         rotation = Quaternion.Euler(0f,-_mouseDelta.normalized.magnitude * _mouseDirX * rotateSpd * Time.deltaTime,0f);
    //     }else 
    //     {
    //         rotation = Quaternion.Euler(_mouseDelta.normalized.magnitude * _mouseDirY *rotateSpd* Time.deltaTime, 0f,0f);
    //     }
    //     
    //     //rotation = Quaternion.Euler(_mouseDelta.y * rotateSpd * Time.deltaTime, -_mouseDelta.x* rotateSpd* Time.deltaTime, rotation.z);
    //
    //     cubeTransform.rotation = rotation * cubeTransform.rotation;
    //
    // }

    private void UpdateInputUsingMouse()
    {
        cubeTransform.rotation = Quaternion.Lerp(cubeTransform.rotation, _targetRotation, Time.deltaTime * rotateSpd);
        if (_currentCubeState != CubeStates.Rotate) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            _initPos = Input.mousePosition;
            isDragging = true;
        }

        if (isDragging)
        {
            if (Input.GetMouseButton(0))
            {
                _mouseDelta = Input.mousePosition - _initPos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (Mathf.Abs(_mouseDelta.x) > Mathf.Abs(_mouseDelta.y))
                {
                    if (_mouseDelta.x > sensitivityThreshold) // right
                    {
                        print("right");
                        //cubeTransform.RotateAround(cubeTransform.position, Vector3.up,  -90);
                        _targetRotation = Quaternion.AngleAxis(-90, Vector3.up) * _prevRotation;
                        _prevRotation = _targetRotation;
                    }
                    else if (_mouseDelta.x < -sensitivityThreshold) // left
                    {    
                        print("left");
                        //cubeTransform.RotateAround(cubeTransform.position, Vector3.up,  90);
                        _targetRotation = Quaternion.AngleAxis(90, Vector3.up) * _prevRotation;
                        _prevRotation = _targetRotation;
                    }
                }
                else
                {
                    if (_mouseDelta.y > sensitivityThreshold) //up
                    {
                        print("up");
                        //cubeTransform.RotateAround(cubeTransform.position, Vector3.right,  90);
                        _targetRotation = Quaternion.AngleAxis(90, Vector3.right) * _prevRotation;
                        _prevRotation = _targetRotation;
                    }
                    else if (_mouseDelta.y < sensitivityThreshold) //down
                    {
                        print("down");
                        //cubeTransform.RotateAround(cubeTransform.position, Vector3.right,  -90);
                        _targetRotation = Quaternion.AngleAxis(-90, Vector3.right) * _prevRotation;
                        _prevRotation = _targetRotation;
                    }
                }
                _mouseDelta = Vector2.zero;
                _initPos = Vector3.zero;
                isDragging = false;
            }
        }
        
        //cubeTransform.rotation = Quaternion.Lerp(cubeTransform.rotation, _targetRotation, Time.deltaTime * rotateSpd);
    }
    
    private bool CheckTouchDelta()
    {
        bool deltaX;

        deltaX = Math.Abs(_touch.deltaPosition.x) > Math.Abs(_touch.deltaPosition.y);

        return deltaX;
    }

    private bool CheckMouseDelta()
    {
        bool deltaX;

        deltaX = Mathf.Abs(_mouseDelta.x) > Mathf.Abs(_mouseDelta.y);

        return deltaX;
    }
    
    //Tap
    private void TapOnInteractables()
    {
        if (_currentCubeState != CubeStates.Examine) return;
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast (ray, out var hit)) {
            
            if (hit.collider){
                if(hit.collider.GetComponent<Interactor>() != null) hit.collider.GetComponent<Interactor>().Interact();
            }
        }
    }

    public void ResetDrag()
    {
        isDragging = false;
    }
}
