using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CubeCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private float examineDistance;
    [SerializeField] private float rotateDistance;

    private CinemachineTransposer _transposer;
    
    private CubeState _cubeState;
    private CubeStates _currentState;
    
    // Start is called before the first frame update
    void Start()
    {
        _cubeState = GetComponent<CubeState>();
        _transposer = mainCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    // Update is called once per frame
    void Update()
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
                _transposer.m_FollowOffset = new Vector3(0, 0, -examineDistance);
                break;
            
            case CubeStates.Rotate:
                _transposer.m_FollowOffset = new Vector3(0, 0, -rotateDistance);
                break;
        }
    }
}
