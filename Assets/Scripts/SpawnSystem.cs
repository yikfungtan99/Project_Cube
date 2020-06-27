using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SpawnSystem : MonoBehaviourPun
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private CinemachineVirtualCamera playerCamera;

    public List<PlayerCube> currentCubes;

    //Cache player cube
    public GameObject playerCube;

    private void Awake()
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

    public void AssignCube()
    {
        print(currentCubes.Count);
        if (currentCubes.Count < 2) return;
        print("Cube assigned successfully");
        currentCubes[0].otherCube = currentCubes[1];
        currentCubes[1].otherCube = currentCubes[0];
    }
}
