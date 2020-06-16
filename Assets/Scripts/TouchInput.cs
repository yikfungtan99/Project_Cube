using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public bool useOnlyTouch = false;

    [SerializeField] private Camera mainCam;
    
    
    [SerializeField] private Transform cubeTransform;
    [SerializeField] private float rotateSpd;
    private Vector3 _initPos;
    private Touch _touch;
    
    float mouseDirX;
    float mouseDirY;

    private Vector2 _mouseDelta;

    private void Start()
    {
        if (!cubeTransform) cubeTransform = GameObject.FindWithTag("SpawnSystem").GetComponent<SpawnSystem>().playerCube.transform;
        
        Input.simulateMouseWithTouches = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputUsingTouchScreen();
        UpdateInputUsingMouse();
        Tap();
    }

    void UpdateInputUsingTouchScreen()
    {
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

    private void UpdateInputUsingMouse()
    {
        if (useOnlyTouch) return;
        if (Input.GetMouseButtonDown(0)) _initPos = Input.mousePosition;

        if (!Input.GetMouseButton(0)) return;
        
        _mouseDelta = Input.mousePosition - _initPos;
        
        if (_mouseDelta.x > 0)
        {
            mouseDirX = 1;
        }
        else
        {
            mouseDirX = -1;
        }

        if (_mouseDelta.y > 0)
        {
            mouseDirY = 1;
        }
        else
        {
            mouseDirY = -1;
        }

        var rotation = cubeTransform.rotation;
        
        if (CheckMouseDelta())
        {
            rotation = Quaternion.Euler(0f,-_mouseDelta.normalized.magnitude * mouseDirX * rotateSpd * Time.deltaTime,0f);
        }else 
        {
            rotation = Quaternion.Euler(_mouseDelta.normalized.magnitude * mouseDirY *rotateSpd* Time.deltaTime, 0f,0f);
        }
        
        //rotation = Quaternion.Euler(_mouseDelta.y * rotateSpd * Time.deltaTime, -_mouseDelta.x* rotateSpd* Time.deltaTime, rotation.z);

        cubeTransform.rotation = rotation * cubeTransform.rotation;

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
    private void Tap()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast (ray, out var hit)) {
            
            if (hit.collider){
                if(hit.collider.GetComponent<IInteractable>() != null) hit.collider.GetComponent<IInteractable>().Interact();
            }
        }
    }
}
