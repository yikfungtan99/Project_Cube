using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RandomizeMaze : MonoBehaviour
{

    public GameObject[] Spawner;
    public GameObject[] Maze;
    public Transform[] SpawnPoint;


    public List<int> TakeList = new List<int>();
    private int randomnumber;

    // Start is called before the first frame update
    void Start()
    {
        TakeList = new List<int>(new int[Spawner.Length]);

        for (int i = 0; i < Spawner.Length; i++)
        {
            randomnumber = UnityEngine.Random.Range(1, (Maze.Length) + 1);

            while (TakeList.Contains(randomnumber))
            {
                randomnumber = UnityEngine.Random.Range(1, (Maze.Length) + 1);
            }

            TakeList[i] = randomnumber;
            Spawner[i] = Maze[(TakeList[i] - 1)];


            Instantiate(Spawner[i], SpawnPoint[0+i].position, transform.rotation);

        }

    }

        // Update is called once per frame
        void Update()
        {

        }
}
