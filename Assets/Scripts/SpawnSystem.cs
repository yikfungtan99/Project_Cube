using System;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class SpawnSystem : MonoBehaviourPun
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private CinemachineVirtualCamera playerCamera;

    //Cache player cube
    public GameObject playerCube;

    private void Start()
    {
        SpawnCubes();
        SetCamera();
    }

    //Spawn Cubes on spawnPoints based on your player number
    private void SpawnCubes()
    {
        int spawnPosNum = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        playerCube = PhotonNetwork.Instantiate(cubePrefab.name, spawnPoints[spawnPosNum].position, Quaternion.identity);
    }

    private void SetCamera()
    {
        playerCamera.Follow = playerCube.transform;
        playerCamera.LookAt = playerCube.transform;
    }
}
