using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CubeCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private float examineDistance;
    [SerializeField] private float rotateDistance;
    [SerializeField] private float zoomSpeed;

    private float _currentDistance;

    private CinemachineTransposer _transposer;
    
    private CubeState _cubeState;
    private CubeStates _currentState;
    
    // Start is called before the first frame update
    private void Start()
    {
        _cubeState = GetComponent<CubeState>();
        _transposer = mainCamera.GetCinemachineComponent<CinemachineTransposer>();
        _currentDistance = _transposer.m_FollowOffset.z;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateCubeState();
        UpdateCamera();
    }

    private void UpdateCubeState()
    {
        _currentState = _cubeState.currentState;
    }

    private void UpdateCamera()
    {
        switch (_currentState)
        {
            case CubeStates.Examine:
                _currentDistance = Mathf.Lerp(_currentDistance, -examineDistance, Time.deltaTime * zoomSpeed);
                break;
            
            case CubeStates.Rotate:
                _currentDistance = Mathf.Lerp(_currentDistance, -rotateDistance, Time.deltaTime * zoomSpeed);
                break;
        }
        _transposer.m_FollowOffset = new Vector3(0,0, _currentDistance);
    }
}
